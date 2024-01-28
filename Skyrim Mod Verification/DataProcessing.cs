using System.IO.Compression;
using System.Text.Json;

namespace SkyrimModVerification
{
	public static class DataProcessing
	{

		/// <summary>
		/// Deserialize Json file values into a FolderLocationInfo object
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public static IInfo GetAsInfo(string jsonData)
		{
			if (!string.IsNullOrWhiteSpace(jsonData))
			{
				try
				{
					return JsonSerializer.Deserialize<FolderLocationInfo>(jsonData) ?? new FolderLocationInfo();
				}
				catch
				{
					Console.WriteLine($"*** Something went wrong trying to deserialize the jsonData.");
					return new FolderLocationInfo();
				}
			}
			return new FolderLocationInfo();
		}

		/// <summary>
		/// Get a collection of strings representing the content of the compressed filename passed in
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetCompressedFileContent(string filePath)
		{
			if (!string.IsNullOrWhiteSpace(filePath))
			{
				try
				{
					var compression = CompressionFactory.GetCompressionType(filePath) ?? throw new Exception();
					var fileList = new List<string>();
					foreach (var file in compression.GetFiles())
					{
						fileList.Add(file);
					}
					return fileList;
				}
				catch
				{
					Console.WriteLine($"*** Something went wrong trying to read from the compressed file {filePath}");
					return new List<string>();
				}
			}
			else
			{
				return new List<string>();
			}

		}

		/// <summary>
		/// Get a list of the compressed files to validate
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetCompressedFileList(IInfo info)
		{
			try
			{
				var dirRar = Directory.EnumerateFiles(info.CompressedSource, "*.rar") ?? new List<string>();
				var dirZip = Directory.EnumerateFiles(info.CompressedSource, "*.zip") ?? new List<string>();
				var dir7z = Directory.EnumerateFiles(info.CompressedSource, "*.7z") ?? new List<string>();
				return dirRar.Concat(dirZip).Concat(dir7z).OrderBy(x => x);
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong trying to read from the compressed file directory {info.CompressedSource}");
				return new List<string>();
			}
		}

		/// <summary>
		/// Get a list of the files in the destination directory with full path info
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetDirectoryFileList(IInfo info)
		{
			try
			{
				var dir = Directory.EnumerateFiles(info.DeployDestination, "", SearchOption.AllDirectories) ?? new List<string>();
				return dir.OrderBy(x => x);
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong trying to read from the file directory {info.DeployDestination}");
				return new List<string>();
			}
		}

		/// <summary>
		/// Get a list of which files in sourceList are missing from destinationList
		/// </summary>
		/// <param name="info"></param>
		/// <param name="sourceList"></param>
		/// <param name="destinationList"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetMissingSourceFiles(IInfo info, IEnumerable<string> sourceList, IEnumerable<string> destinationList)
		{
			var dest = TransformList(destinationList, info.DeployDestination);

			var missingList = new List<string>();
			foreach (var file in sourceList)
			{
				var contents = TransformList(GetCompressedFileContent(file), info.CompressedSource);
				missingList.AddRange(contents.Where(x => !dest.Contains(x)));
			}
			return missingList;
		}

		/// <summary>
		/// Validate Source and Destination values are not empty
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool IsValidInfo(IInfo info)
		{
			return !(info == null || string.IsNullOrWhiteSpace(info.CompressedSource) || string.IsNullOrWhiteSpace(info.DeployDestination));
		}

		/// <summary>
		/// Read the json data from a file into a string
		/// </summary>
		/// <param name="jsonPath"></param>
		/// <returns></returns>
		public static string ReadPathInfo(string jsonPath = "ConfigLocations.json")
		{
			if (!string.IsNullOrWhiteSpace(jsonPath))
			{
				try
				{
					var path = Path.Combine(Environment.CurrentDirectory, jsonPath);
					using var jsonStream = new StreamReader(path);
					var readData = jsonStream.ReadToEnd();
					Console.WriteLine(readData);
					return readData;
				}
				catch
				{
					Console.WriteLine($"*** Something went wrong trying to read the json configuration file.");
					return "";
				}
			}

			return "";
		}

		/// <summary>
		/// Removes the text of removeVal from each item in list
		/// </summary>
		/// <param name="list"></param>
		/// <param name="removeVal"></param>
		/// <returns></returns>
		public static IEnumerable<string> TransformList(IEnumerable<string> list, string removeVal)
		{
			return list.Select(y => y.Replace(removeVal, ""));
		}
	}
}