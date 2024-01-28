using System.IO.Compression;
using System.Text.Json;

namespace Skyrim_Mod_Verification
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
					using var compressedData = ZipFile.OpenRead(filePath);
					var fileList = new List<string>();
					foreach (var file in compressedData.Entries)
					{
						fileList.Add(file.FullName);
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
			if (!string.IsNullOrWhiteSpace(jsonPath)) try
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
			return "";
		}
	}
}