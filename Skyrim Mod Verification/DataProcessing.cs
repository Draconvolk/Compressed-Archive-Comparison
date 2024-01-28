using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Skyrim_Mod_Verification
{
	public static class DataProcessing
	{
		/// <summary>
		/// Read the json data from a file into a string
		/// </summary>
		/// <param name="jsonPath"></param>
		/// <returns></returns>
		public static string ReadPathInfo(string jsonPath = "ConfigLocations.json")
		{
			var path = Path.Combine(Environment.CurrentDirectory, jsonPath);
			using var jsonStream = new StreamReader(path);
			var readData = jsonStream.ReadToEnd();
			Console.WriteLine(readData);
			return readData;
		}

		/// <summary>
		/// Deserialize Json file values into a FolderLocationInfo object
		/// </summary>
		/// <param name="jsonData"></param>
		/// <returns></returns>
		public static FolderLocationInfo GetAsInfo(string jsonData)
		{
			if (!string.IsNullOrWhiteSpace(jsonData))
			{
				try
				{
					return JsonSerializer.Deserialize<FolderLocationInfo>(jsonData) ?? new FolderLocationInfo() { CompressedSource = "", DeployDestination = "" };
				}
				catch
				{
					return new FolderLocationInfo() { CompressedSource = "", DeployDestination = "" };
				}
			}
			return new FolderLocationInfo() { CompressedSource = "", DeployDestination = "" };
		}

		/// <summary>
		/// Validate Source and Destination values are not empty
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool IsValidInfo(FolderLocationInfo info)
		{
			return !(info == null || string.IsNullOrWhiteSpace(info.CompressedSource) || string.IsNullOrWhiteSpace(info.DeployDestination));
		}

		/// <summary>
		/// Get a list of the compressed files to validate
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetCompressedFileList(FolderLocationInfo info)
		{
			try
			{
				var dirRar = Directory.EnumerateFiles(info.CompressedSource, "*.rar") ?? new List<string>();
				var dirZip = Directory.EnumerateFiles(info.CompressedSource, "*.zip") ?? new List<string>();
				var dir7z = Directory.EnumerateFiles(info.CompressedSource, "*.7z") ?? new List<string>();
				return dirRar.Concat(dirZip).Concat(dir7z);
			}
			catch
			{
				return new List<string>();
			}
		}
	}
}
