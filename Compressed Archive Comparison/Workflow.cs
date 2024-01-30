namespace CompressedArchiveComparison
{
	public class Workflow(IInfo info)
	{
		private const string source = "source";

		public string Config { get; set; } = "Config.json";
		public IInfo ConfigInfo { get; set; } = info;
		public IEnumerable<string> SourceList { get; set; } = [];
		public IEnumerable<string> DestinationList { get; set; } = [];
		public IEnumerable<string> MissingFiles { get; set; } = [];

		public void SetConfig(string[] args, string defaultConfig)
		{
			var configName = defaultConfig;
			if (args.Length != 0)
			{
				configName = args[0];
				configName = string.IsNullOrWhiteSpace(configName?.Trim()) ? defaultConfig : configName.Trim();
			}
			Config = configName;
			Console.WriteLine("Config name set");
		}

		public void LoadConfig()
		{
			ConfigInfo = GetInfoFromJson(Config);
			Console.WriteLine("Config found");
		}

		public async Task LoadCompressedSource()
		{
			SourceList = await GetFileList(ConfigInfo);
			Console.WriteLine("Compressed Files Found");
		}

		public async Task DisplaySource()
			=> await PrintFileList(SourceList, ConfigInfo);

		public async Task LoadDestination()
		{
			DestinationList = await DataProcessing.GetDirectoryFileList(ConfigInfo);
			Console.WriteLine("Destination Found");
		}

		public async Task IdentifyMissingFiles()
			=> MissingFiles = await DataProcessing.GetMissingSourceFiles(SourceList, DestinationList);

		public async Task DisplayMissingFiles()
			=> await PrintFileList(MissingFiles, ConfigInfo, "missing");

		public async Task ExportMissingFiles()
			=> await ExportToFile(MissingFiles, ConfigInfo);

		public static IInfo GetInfoFromJson(string configName)
		{
			var readData = DataProcessing.ReadPathInfo(configName);
			var ConfigInfo = DataProcessing.GetAsInfo(readData);
			return ConfigInfo;
		}

		public static async Task<IEnumerable<string>> GetFileList(IInfo ConfigInfo)
		{
			if (!DataProcessing.IsValidInfo(ConfigInfo))
			{
				Console.WriteLine("Invalid Source and Destination data in Config file. Please specify valid paths and try again.");
				Console.WriteLine();
				throw new Exception("Invalid Source or Destination data in config file.");
			}

			Console.WriteLine();
			Console.WriteLine($"Src: {ConfigInfo.CompressedSource}");
			Console.WriteLine($"Dest: {ConfigInfo.DeployDestination}");
			Console.WriteLine();
			var fileList = await DataProcessing.GetCompressedFileList(ConfigInfo);
			return fileList;
		}

		public static async Task PrintFileList(IEnumerable<string> fileList, IInfo info, string type = source)
		{
			foreach (var file in fileList)
			{
				Console.WriteLine(file);
				var fileContents = type == source ? await DataProcessing.GetCompressedFileContent(file) : new List<string>();

				if (info.Verbose)
				{
					await Task.Run(() =>
					{
						foreach (var contents in fileContents)
						{
							Console.WriteLine($"   {contents}");
						}
					});
				}

				if (type == source)
				{
					Console.WriteLine();
				}
			}
		}

		public static async Task ExportToFile(IEnumerable<string> fileList, IInfo info)
		{
			if (await DataProcessing.WriteToFile(fileList, info.ExportFileName))
			{
				Console.WriteLine();
				Console.WriteLine("Missing File List export successful");
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine("*** An error was encountered attempting to export the Missing File List");
				Console.WriteLine();
			}
		}
	}
}
