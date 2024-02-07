using CompressedArchiveComparison.Config;
using CompressedArchiveComparison.Exceptions;
using System.Collections.Concurrent;
using System.Text.Json;

namespace CompressedArchiveComparison.Components
{
	public static class DataProcessing
	{
		private static readonly string DP = "DataProcessing";

		/// <summary>
		/// Valid File Extensions for Output File to be written to
		/// </summary>
		public static readonly List<string> ValidFileExtensions = [".log", ".txt"];
		public static readonly string DefaultSeparator = Environment.NewLine;
		public static readonly string fileSeparator = "|";

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
			try
			{
				if (sourceList == null || !sourceList.Any() || exclusionList == null || !exclusionList.Any())
				{
					return filteredList;
				}
				filteredList = [.. sourceList.Where(x => !exclusionList.Contains(GetFileName(x))).OrderBy(x => x)];
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong trying to filter the source list", $"{DP}\\FilterSourceList");
			}
			return filteredList;
		}

		/// <summary>
		/// Deserialize Json text <paramref name="jsonData"/> into a <see cref="ConfigurationInfo"/> object
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public static IInfo GetAsInfo(string jsonData)
		{
			if (string.IsNullOrWhiteSpace(jsonData))
			{
				ExceptionList.Add(new Exception("Blank Data"), "JSON data is empty", $"{DP}\\GetAsInfo", [jsonData]);
				return new ConfigurationInfo();
			}
			try
			{
				return JsonSerializer.Deserialize<ConfigurationInfo>(jsonData) ?? new ConfigurationInfo();
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Error deserializing JSON data", $"{DP}\\GetAsInfo", [jsonData]);
				return new ConfigurationInfo();
			}
		}

		/// <summary>
		/// Get a collection of <see cref="string"/> representing the content of the compressed filename <paramref name="filePath"/>
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetCompressedFileContent(CompressionResolver resolver, string filePath)
		{
			var files = new List<string>();
			if (string.IsNullOrWhiteSpace(filePath))
			{
				ExceptionList.Add(new Exception("Filename is empty"), "The Filename is empty", $"{DP}\\GetAsInfo", [filePath]);
				return files;
			}
			try
			{
				var compression = CompressionFactory.GetCompressionType(resolver, filePath);
				if (compression == null) { return files; }
				Parallel.ForEach(compression.GetFiles(), files.Add);
				return files.OrderBy(x => x);
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Error reading compressed file content", $"{DP}\\GetCompressedFileContent", [filePath]);
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
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong getting list of compressed files from the source directory", $"{DP}\\GetCompressedFileList", [info.CompressedSource]);
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
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong getting list of files from the source directory", $"{DP}\\GetCompressedOfType", [info.CompressedSource, type]);
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
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong getting list of files from the destination directory", $"{DP}\\GetDirectoryFileList", [info.DeployDestination]);
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
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong trying to read the exclusion file", $"{DP}\\GetExclusionFileText", [info.ExclusionsFileName]);
				return "";
			}
		}

		/// <summary>
		/// Get a collection of the names of files in <paramref name="sourceList"/> which are missing from <paramref name="destinationList"/>
		/// </summary>
		/// <param name="sourceList"></param>
		/// <param name="destinationList"></param>
		/// <returns></returns>		
		public static IEnumerable<string> GetMissingSourceFiles(CompressionResolver resolver, IEnumerable<string> sourceList, IEnumerable<string> destinationList)
		{
			var missingFileBag = new ConcurrentBag<string>();
			var missingTasks = new List<Task>();
			try
			{
				Parallel.ForEach(sourceList, file =>
					missingTasks.Add(Task.Run(async () =>
					{
						var files = await DetermineMissingFiles(resolver, file, destinationList);
						foreach (var item in files)
						{
							missingFileBag.Add(item);
						}
					}))
				);
				Task.WaitAll([.. missingTasks]);
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong trying to create the list of missing files", $"{DP}\\GetMissingSourceFiles");
				return [];
			}

			var executeMissingTasks = new List<Task>();
			var missingList = new ConcurrentBag<string>();
			var addMissing = new object();
			try
			{
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
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong trying to create the list of missing files", $"{DP}\\GetMissingSourceFiles");
				return [];
			}
		}

		/// <summary>		
		/// Get a collection of the names of files from inside the specified compressed <paramref name="file"/> 
		/// and compares that against the <paramref name="destinationList"/> and returns a collection of those missing
		/// </summary>
		/// <param name="file"></param>
		/// <param name="destinationList"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<string>> DetermineMissingFiles(CompressionResolver resolver, string file, IEnumerable<string> destinationList)
		{
			try
			{
				IEnumerable<string> justCompressedFiles = [];
				var getCompressedTask = Task.Factory.StartNew(() =>
				{
					var compressedFileContent = GetCompressedFileContent(resolver, file);
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
				var missingFiles = FilterMissingFiles(targetDestList, justCompressedFiles);
				var fullFilePath = AddPathToValue(missingFiles, file, fileSeparator);
				return fullFilePath;
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong trying to determine the missing files", $"{DP}\\DetermineMissingFiles", [file]);
				return [];
			}
		}

		/// <summary>
		/// Remove directory only rows and return only rows with filenames
		/// </summary>
		/// <param name="compressedFileContent"></param>
		/// <returns></returns>
		public static IEnumerable<string> OnlyFiles(IEnumerable<string> compressedFileContent)
		{
			if (compressedFileContent == null)
			{
				ExceptionList.Add(new Exception("Filename collection is null"), "The filename collection is null", $"{DP}\\OnlyFiles");
				yield break;
			}
			var fileContents = compressedFileContent.ToArray();
			if (fileContents.Length <= 1)
			{
				if (fileContents.Length == 0)
				{
					yield break;
				}
				if (fileContents.Length == 1)
				{
					if (fileContents[0].Contains('.'))
					{
						yield return fileContents[0];
					}
				}
			}
			for (var index = 0; index < fileContents.Length - 1; index++)
			{
				if (fileContents[index + 1].Contains(fileContents[index]))
				{
					continue;
				}
				if (fileContents[index].Contains('.'))
				{
					yield return fileContents[index];
				}
			}
			var last = fileContents.Last();
			if (last.Contains('.'))
			{
				yield return last;
			}
		}

		/// <summary>
		/// Returns the filename, stripped of additional path 
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string GetFileName(string file)
		{
			try
			{
				return file[(file.LastIndexOf('\\') + 1)..];
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong getting just the filename", $"{DP}\\GetFileName", [file]);
				return "";
			}
		}

		/// <summary>
		/// Returns the <paramref name="fileName"/> stripped of its extension for use as a foldername
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string GetFolderName(string fileName)
		{
			try
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
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong getting the foldername", $"{DP}\\GetFolderName", [fileName]);
				return "";
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
				try
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
				}
				catch (Exception ex)
				{
					ExceptionList.Add(ex, "Something went wrong filtering the destination list", $"{DP}\\FilterDestination", [targetFolder]);
					return [];
				}
			});
		}

		/// <summary>
		/// Returns a collection of <paramref name="justCompressedFiles"/> values that don't exist in <paramref name="targetDestList"/>
		/// </summary>
		/// <param name="targetDestList"></param>
		/// <param name="justCompressedFiles"></param>
		/// <returns></returns>
		public static IEnumerable<string> FilterMissingFiles(IEnumerable<string> targetDestList, IEnumerable<string> justCompressedFiles)
		{
			var missingFiles = new List<string>();
			try
			{
				foreach (var file in justCompressedFiles)
				{
					if (!targetDestList.Any(y => y.Contains(file)))
					{
						missingFiles.Add(file);
					}
				}
				return missingFiles;
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong filtering the missing files list", $"{DP}\\FilterMissingFiles");
				return [];
			}
		}

		/// <summary>
		/// Adds the full <paramref name="addPath"/> value to each item in <paramref name="list"/> with the specified <paramref name="separator"/>
		/// </summary>
		/// <param name="list"></param>
		/// <param name="addPath"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static IEnumerable<string> AddPathToValue(IEnumerable<string> list, string addPath, string separator)
		{
			try
			{
				return list.Select(y => $"{addPath}{separator}{y}");
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong adding the specified filePath to the list", $"{DP}\\AddPathToValue", [addPath, separator]);
				return [];
			}
		}

		/// <summary>
		/// Validate <see cref="IInfo.CompressedSource"/> and <see cref="IInfo.DeployDestination"/> values are not empty
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool IsValidInfo(IInfo info)
			=> !(info == null
			|| string.IsNullOrWhiteSpace(info.CompressedSource)
			|| string.IsNullOrWhiteSpace(info.DeployDestination)
			|| string.IsNullOrWhiteSpace(info.ExclusionsFileName)
			|| string.IsNullOrWhiteSpace(info.ExportFileName));

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
			catch (Exception ex)
			{
				ExceptionList.Add(ex, "Something went wrong parsing the exclusion file text", $"{DP}\\ParseExclusionFileText", [exclusionText, separator]);
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
			if (string.IsNullOrWhiteSpace(jsonPath))
			{
				return "";
			}
			try
			{
				var path = Path.Combine(Environment.CurrentDirectory, jsonPath);
				var readData = await ReadFileData(path);
				Console.WriteLine(readData);
				return readData;
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, $"Error trying to read theJSON data from {jsonPath}", $"{DP}\\ReadPathInfo", [jsonPath]);
				return "";
			}
		}

		/// <summary>
		/// Reads the text of the file <paramref name="filePath"/> as a single string
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static async Task<string> ReadFileData(string filePath)
		{
			try
			{
				var jsonStream = new StreamReader(filePath);
				var readData = await jsonStream.ReadToEndAsync();
				return readData;
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, $"Error trying to read from the file {filePath}", $"{DP}\\ReadFileData", [filePath]);
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
					ExceptionList.Add(new Exception("Invalid filepath"), $"Invalid filepath for output file", $"{DP}\\WriteToFile", [filePath]);
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
			catch (Exception ex)
			{
				ExceptionList.Add(ex, $"Error trying to write the missing files to {filePath}", $"{DP}\\WriteToFile", [filePath]);
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
			try
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
					var validExt = "";

					foreach (var ext in ValidFileExtensions)
					{
						validExt += ext + " ";
					}

					ExceptionList.Add(new Exception("Invalid extension"), $"Invalid filename extension for the file {filePath}. Valid extensions are: {validExt}", $"{DP}\\NormalizeFileName", [filePath]);
					return "";
				}
				return Path.GetFullPath(filePath);
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, $"Something went wrong trying to normalize the filename {filePath}", $"{DP}\\NormalizeFileName", [filePath]);
				return "";
			}
		}
	}
}