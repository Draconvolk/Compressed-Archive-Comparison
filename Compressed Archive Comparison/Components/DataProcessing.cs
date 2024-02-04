using CompressedArchiveComparison.Config;
using System.Collections.Concurrent;
using System.Text.Json;

namespace CompressedArchiveComparison.Components
{
	public static class DataProcessing
	{
		/// <summary>
		/// Valid File Extensions for Output File to be written to
		/// </summary>
		public static readonly List<string> ValidFileExtensions = [".log", ".txt"];
		public static readonly string DefaultSeparator = Environment.NewLine;

		/// <summary>
		/// Returns a collection of <paramref name="sourceList"/> values that don't exist in the <paramref name="exclusionList"/>,
		/// or an empty string collection
		/// </summary>
		/// <param name="sourceList"></param>
		/// <param name="exclusionList"></param>
		/// <returns></returns>
		public static IEnumerable<string> FilterSourceList(IEnumerable<string> sourceList, IEnumerable<string> exclusionList)
		{
			var filteredList = new List<string>();
			if (sourceList == null || !sourceList.Any() || exclusionList == null || !exclusionList.Any())
			{
				return filteredList;
			}
			filteredList = [.. sourceList.Where(x => !exclusionList.Contains(GetFileName(x))).OrderBy(x => x)];

			return filteredList;
		}

		/// <summary>
		/// Deserialize Json text <paramref name="jsonData"/> into a <see cref="ConfigurationInfo"/> object
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
		/// Get a collection of  <see cref="string"/>s representing the content of the compressed filename <paramref name="filePath"/>
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetCompressedFileContent(string filePath)
		{
			var files = new List<string>();
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return files;
			}
			try
			{
				var compression = CompressionFactory.GetCompressionType(filePath);
				if (compression == null) { return files; }
				Parallel.ForEach(compression.GetFiles(), files.Add);
				return files.OrderBy(x => x);
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong trying to read from the compressed file {filePath}");
				return files;
			}
		}

		/// <summary>
		/// Get a collection of the compressed files
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetCompressedFileList(IInfo info)
		{
			var dirData = new List<string>();
			try
			{
				dirData.AddRange(GetCompressedOfType(info, "*.rar"));
				dirData.AddRange(GetCompressedOfType(info, "*.zip"));
				dirData.AddRange(GetCompressedOfType(info, "*.7z"));
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong trying to read from the compressed file directory {info.CompressedSource}");
			}
			return dirData.OrderBy(x => x);
		}

		/// <summary>
		/// Get a collection of the names of all files of <paramref name="type"/> found in the <see cref="IInfo.CompressedSource"/> location
		/// </summary>
		/// <param name="info"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetCompressedOfType(IInfo info, string type)
		{
			var files = new List<string>();
			try
			{
				files = Directory.EnumerateFiles(info.CompressedSource, type).ToList();
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong trying to read from the compressed file directory {info.CompressedSource}");
			}
			return files;
		}

		/// <summary>
		/// Get a collection of the names of the files in the <see cref="IInfo.DeployDestination"/> directory with full path info
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetDirectoryFileList(IInfo info)
		{
			var dir = new List<string>();
			try
			{
				var files = Directory.EnumerateFiles(info.DeployDestination, "", SearchOption.AllDirectories);
				foreach (var file in files)
				{
					dir.Add(file);
				}
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong trying to read from the file directory {info.DeployDestination}");
			}
			return dir;
		}

		/// <summary>
		/// Read the text content of the file <see cref="IInfo.ExclusionsFileName"/>
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static async Task<string> GetExclusionFileText(IInfo info)
		{
			try
			{
				var file = new FileInfo(info.ExclusionsFileName);
				var excludeText = await ReadFileData(file.FullName);
				return excludeText;
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong trying to read the exclusion file [{info.ExclusionsFileName}]");
				return "";
			}
		}

		/// <summary>
		/// Get a collection of the names of files in <paramref name="sourceList"/> which are missing from <paramref name="destinationList"/>
		/// </summary>
		/// <param name="sourceList"></param>
		/// <param name="destinationList"></param>
		/// <returns></returns>		
		public static IEnumerable<string> GetMissingSourceFiles(IEnumerable<string> sourceList, IEnumerable<string> destinationList)
		{
			var missingFileBag = new ConcurrentBag<string>();
			var missingTasks = new List<Task>();
			Parallel.ForEach(sourceList, file =>
				missingTasks.Add(Task.Run(async () =>
				{
					var files = await DetermineMissingFiles(file, destinationList);
					foreach (var item in files)
					{
						missingFileBag.Add(item);
					}
				}))
			);
			Task.WaitAll([.. missingTasks]);

			var executeMissingTasks = new List<Task>();
			var missingList = new ConcurrentBag<string>();
			var addMissing = new object();
			while (!missingFileBag.IsEmpty)
			{
				executeMissingTasks.Add(Task.Run(() =>
				{
					if (missingFileBag.TryTake(out var missingFile))
					{
						lock (addMissing)
						{
							missingList.Add(missingFile);
						}
					}
				}));
			}
			Task.WaitAll([.. executeMissingTasks]);

			return missingList.OrderBy(x => x);
		}

		/// <summary>		
		/// Get a collection of the names of files from inside the specified compressed <paramref name="file"/> 
		/// and compares that against the <paramref name="destinationList"/> and returns a collection of those missing
		/// </summary>
		/// <param name="file"></param>
		/// <param name="destinationList"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<string>> DetermineMissingFiles(string file, IEnumerable<string> destinationList)
		{
			IEnumerable<string> justCompressedFiles = [];
			var getCompressedTask = Task.Factory.StartNew(() =>
			{
				var compressedFileContent = GetCompressedFileContent(file);
				justCompressedFiles = OnlyFiles(compressedFileContent);
			});
			IEnumerable<string> targetDestList = [];
			var getDestTask = Task.Run(async () =>
			{
				var fileName = GetFileName(file);
				var targetFolder = GetFolderName(fileName);
				targetDestList = await FilterDestination(destinationList, targetFolder);
			});
			await Task.WhenAll([getCompressedTask, getDestTask]);
			var missingFiles = await FilterMissingFiles(targetDestList, justCompressedFiles);
			var fullFilePath = AddPathToValue(missingFiles, file, "|");
			return fullFilePath;
		}

		/// <summary>
		/// Remove directory only rows and return only rows with filenames
		/// </summary>
		/// <param name="compressedFileContent"></param>
		/// <returns></returns>
		public static IEnumerable<string> OnlyFiles(IEnumerable<string> compressedFileContent)
		{
			foreach (var file in compressedFileContent)
			{
				if (file.Contains('.'))
				{
					yield return file;
				}
			};
		}

		/// <summary>
		/// Returns the filename, stripped of additional path 
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string GetFileName(string file)
			=> file[(file.LastIndexOf('\\') + 1)..];

		/// <summary>
		/// Returns the file name stripped of its extension
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string GetFolderName(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return "";
			}
			if (fileName.LastIndexOf('.') == -1)
			{
				return fileName;
			}
			else
			{
				return fileName[..fileName.LastIndexOf('.')];
			}
		}

		/// <summary>
		/// Returns a collection of <paramref name="destinationList"/> values that contain <param
		/// </summary>
		/// <param name="destinationList"></param>
		/// <param name="targetFolder"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<string>> FilterDestination(IEnumerable<string> destinationList, string targetFolder)
		{
			return await Task.Factory.StartNew(() =>
			{
				var filtered = new List<string>();
				foreach (var file in destinationList)
				{
					if (file.Contains(targetFolder))
					{
						filtered.Add(file);
					}
				}
				return filtered;
			});
		}

		/// <summary>
		/// Returns a collection of <paramref name="justCompressedFiles"/> values that don't exist in <paramref name="targetDestList"/>
		/// </summary>
		/// <param name="targetDestList"></param>
		/// <param name="justCompressedFiles"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<string>> FilterMissingFiles(IEnumerable<string> targetDestList, IEnumerable<string> justCompressedFiles)
		{
			return await Task.Run(() =>
			{
				var missingFiles = new List<string>();
				foreach (var file in justCompressedFiles)
				{
					if (!targetDestList.Any(y => y.Contains(file)))
					{
						missingFiles.Add(file);
					}
				}
				return missingFiles;
			});
		}

		/// <summary>
		/// Adds the full <paramref name="addPath"/> value to each item in <paramref name="list"/> with the specified <paramref name="separator"/>
		/// </summary>
		/// <param name="list"></param>
		/// <param name="addPath"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static IEnumerable<string> AddPathToValue(IEnumerable<string> list, string addPath, string separator)
			=> list.Select(y => $"{addPath}{separator}{y}");

		/// <summary>
		/// Validate <see cref="IInfo.CompressedSource"/> and <see cref="IInfo.DeployDestination"/> values are not empty
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool IsValidInfo(IInfo info)
			=> !(info == null || string.IsNullOrWhiteSpace(info.CompressedSource) || string.IsNullOrWhiteSpace(info.DeployDestination));

		/// <summary>
		/// Returns a collection of parsed values from <paramref name="exclusionText"/> separated by <paramref name="separator"/>
		/// </summary>
		/// <param name="exclusionText"></param>
		/// <returns></returns>
		public static IEnumerable<string> ParseExclusionFileText(string exclusionText, string separator)
		{
			var list = new List<string>();
			separator = string.IsNullOrEmpty(separator) ? DefaultSeparator : separator;
			try
			{
				if (!string.IsNullOrWhiteSpace(exclusionText))
				{
					list.AddRange(exclusionText.Split(separator));
				}
			}
			catch
			{
				Console.WriteLine("*** Something went wrong parsing the exclusion file text");
			}
			return list;
		}

		/// <summary>
		/// Read the JSON data from <paramref name="jsonPath"/> into a string
		/// </summary>
		/// <param name="jsonPath"></param>
		/// <returns></returns>
		public static async Task<string> ReadPathInfo(string jsonPath)
		{
			if (!string.IsNullOrWhiteSpace(jsonPath))
			{
				try
				{
					var path = Path.Combine(Environment.CurrentDirectory, jsonPath);
					var readData = await ReadFileData(path);
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
		/// Reads the text of the file <paramref name="path"/> as a single string
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static async Task<string> ReadFileData(string path)
		{
			try
			{
				var jsonStream = new StreamReader(path);
				var readData = await jsonStream.ReadToEndAsync();
				return readData;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"*** Error Reading from file [{path}] - {ex.Message}");
				return "";
			}
		}

		/// <summary>
		/// Writes the collection <paramref name="asyncList"/> to file <paramref name="filePath"/>, one record per line
		/// </summary>
		/// <param name="list"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static async Task<bool> WriteToFile(IEnumerable<string> asyncList, string filePath)
		{
			filePath = NormalizeFileName(filePath);

			try
			{
				if (string.IsNullOrWhiteSpace(filePath))
				{
					Console.WriteLine("Invalid filepath for output file, []");
					return false;
				}
				if (!asyncList.Any())
				{
					await File.WriteAllTextAsync(filePath, "No records were found to be missing!");
					return true;
				}
				await File.WriteAllTextAsync(filePath, "The Following files were missing from the destination:" + Environment.NewLine);
				foreach (var file in asyncList)
				{
					await File.AppendAllTextAsync(filePath, file + Environment.NewLine);
				}
				Console.WriteLine($"Successfuly wrote missing files to output file [{filePath}]");
				return true;
			}
			catch
			{
				Console.WriteLine($"*** Something went wrong while trying to write the missing files to the log file [{filePath}]");
				return false;
			}
		}

		/// <summary>
		/// Returns a normalized full path for the specified filename <paramref name="filePath"/> to export to,
		/// or "" if <paramref name="filePath"/> not of type .log or .txt
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