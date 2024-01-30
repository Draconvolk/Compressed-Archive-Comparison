using System.Text.Json;

namespace CompressedArchiveComparison
{
	public static class DataProcessing
	{
		/// <summary>
		/// Valid File Extensions for Output File to be written to
		/// </summary>
		public static readonly List<string> ValidFileExtensions = [".log", ".txt"];

		/// <summary>
		/// Deserialize Json file values into a ConfigurationInfo object
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public static IInfo GetAsInfo(string jsonData)
		{
			if (!string.IsNullOrWhiteSpace(jsonData))
			{
				try
				{
					return JsonSerializer.Deserialize<ConfigurationInfo>(jsonData) ?? new ConfigurationInfo();
				}
				catch
				{
					Console.WriteLine($"*** Something went wrong trying to deserialize the jsonData.");
					return new ConfigurationInfo();
				}
			}
			return new ConfigurationInfo();
		}

		/// <summary>
		/// Get a collection of strings representing the content of the compressed filename passed in
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<string>> GetCompressedFileContent(string filePath)
		{
			if (!string.IsNullOrWhiteSpace(filePath))
			{
				try
				{
					var compression = CompressionFactory.GetCompressionType(filePath) ?? throw new Exception();
					var files = await compression.GetFiles();
					return files;
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
		public static async Task<IEnumerable<string>> GetCompressedFileList(IInfo info)
		{
			try
			{
				return await Task.Run(() =>
				{
					var dirRar = Directory.EnumerateFiles(info.CompressedSource, "*.rar") ?? new List<string>();
					var dirZip = Directory.EnumerateFiles(info.CompressedSource, "*.zip") ?? new List<string>();
					var dir7z = Directory.EnumerateFiles(info.CompressedSource, "*.7z") ?? new List<string>();
					return dirRar.Concat(dirZip).Concat(dir7z).OrderBy(x => x);
				});
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
		public static async Task<IEnumerable<string>> GetDirectoryFileList(IInfo info)
		{
			try
			{
				return await Task.Run(() =>
				{
					var dir = Directory.EnumerateFiles(info.DeployDestination, "", SearchOption.AllDirectories) ?? new List<string>();
					return dir.OrderBy(x => x);
				});
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
		/// <param name="sourceList"></param>
		/// <param name="destinationList"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<string>> GetMissingSourceFiles(IEnumerable<string> sourceList, IEnumerable<string> destinationList)
		{
			//var dest = FullPathToRelative(destinationList, info.DeployDestination);
			var missingList = new List<string>();
			foreach (var file in sourceList)
			{
				var compressedFileContent = await GetCompressedFileContent(file);
				var justCompressedFiles = OnlyFiles(compressedFileContent);
				var fileName = GetFileName(file);
				var targetFolder = fileName[..fileName.LastIndexOf('.')];
				var targetDestList = FilterDestination(destinationList, targetFolder);
				var missingFiles = justCompressedFiles.Where(x => !targetDestList.Any(y => y.Contains(x)));
				var fullFilePath = AddPathToValue(missingFiles, file, "|");
				missingList.AddRange(fullFilePath);
			}
			return missingList.OrderBy(x => x);
		}

		/// <summary>
		/// Remove directory only rows and return only rows with filenames
		/// </summary>
		/// <param name="compressedFileContent"></param>
		/// <returns></returns>
		public static IEnumerable<string> OnlyFiles(IEnumerable<string> compressedFileContent) 
			=> compressedFileContent.Where(x => x.Contains('.'));

		/// <summary>
		/// Returns just the filename, stripped of additional path 
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string GetFileName(string file)
			=> file[(file.LastIndexOf('\\') + 1)..];


		/// <summary>
		/// Filters the destinationList to only records including the targetFolder
		/// </summary>
		/// <param name="destinationList"></param>
		/// <param name="targetFolder"></param>
		/// <returns></returns>
		public static IEnumerable<string> FilterDestination(IEnumerable<string> destinationList, string targetFolder)
			=> destinationList.Where(x => x.Contains(targetFolder));

		/// <summary>
		/// Validate Source and Destination values are not empty
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool IsValidInfo(IInfo info)
			=> !(info == null || string.IsNullOrWhiteSpace(info.CompressedSource) || string.IsNullOrWhiteSpace(info.DeployDestination));

		/// <summary>
		/// Read the json data from a file into a string
		/// </summary>
		/// <param name="jsonPath"></param>
		/// <returns></returns>
		public static string ReadPathInfo(string jsonPath)
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
		/// Applies FullPathToRelativeTextReplacement on each item in list
		/// </summary>
		/// <param name="list"></param>
		/// <param name="removeText"></param>
		/// <param name="targetFolder"></param>
		/// <returns></returns>
		public static IEnumerable<string> FullPathToRelative(IEnumerable<string> list, string removeText, string targetFolder = "")
			=> list.Select(y => FullPathToRelativeTextReplacement(y, removeText, targetFolder));

		/// <summary>
		/// Removes the text of removeText from text and adds targetFolderas a prefix
		/// </summary>
		/// <param name="text"></param>
		/// <param name="removePath"></param>
		/// <param name="targetFolder"></param>
		/// <returns></returns>
		public static string FullPathToRelativeTextReplacement(string text, string removePath, string targetFolder = "")
			=> $"{targetFolder}{text.Replace(removePath, "")}";

		/// <summary>
		/// Remove compressed files found in the destination list so the list can be shortened for the next search
		/// </summary>
		/// <param name="info"></param>
		/// <param name="destinationList"></param>
		/// <param name="foundFiles"></param>
		/// <returns></returns>
		public static List<string> RemoveFoundFiles(IInfo info, List<string> destinationList, IEnumerable<string> foundFiles)
			=> destinationList.Where(x => !foundFiles.Contains(FullPathToRelativeTextReplacement(x, info.DeployDestination))).ToList();

		/// <summary>
		/// Adds the full path value to each item in the list with the specified separator
		/// </summary>
		/// <param name="list"></param>
		/// <param name="addPath"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static IEnumerable<string> AddPathToValue(IEnumerable<string> list, string addPath, string separator)
			=> list.Select(y => $"{addPath}{separator}{y}");

		/// <summary>
		/// Writes the referenced list of string names to the file 
		/// </summary>
		/// <param name="list"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static async Task<bool> WriteToFile(IEnumerable<string> list, string filePath)
		{
			if (list == null || !list.Any())
			{
				Console.WriteLine("No records found missing. Nothing to Export");
				return false;
			}
			filePath = NormalizeFileName(filePath);
			if (filePath == "")
			{
				return false;
			}
			try
			{
				await File.WriteAllTextAsync(filePath, "The Following files were missing from the destination:" + Environment.NewLine);
				await File.AppendAllLinesAsync(filePath, list);
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong while trying to write the missing files to the log file [{filePath}]");
				return false;
			}
			Console.WriteLine($"Successfuly wrote missing files to output file [{filePath}]");
			return true;
		}

		/// <summary>
		/// Returns a normalized full path for the specified filename to export to, or "" if a filepath not of type .log or .txt is given
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string NormalizeFileName(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				filePath = "MissingFilesFound.txt";
			}
			if (filePath.Length > 0 && !filePath.Contains('.'))
			{
				filePath = filePath.TrimEnd() + ".txt";
			}

			var tmpFile = new FileInfo(filePath);
			if (!ValidFileExtensions.Contains(tmpFile.Extension.ToLower()))
			{
				Console.Write($"*** Invalid FileName Extension for output file [{tmpFile.Extension}]. Please use one of the following: ");
				foreach (var ext in ValidFileExtensions)
				{

					Console.Write(ext + " ");
				}
				Console.WriteLine();
				return "";
			}
			return Path.GetFullPath(filePath);
		}
	}
}