using CompressedArchiveComparison.Config;

namespace CompressedArchiveComparison.Components
{
    public class Workflow(IInfo info): IWorkflow
    {
        private const string source = "source";

        public string Config { get; set; } = "Config.json";
        public IInfo ConfigInfo { get; set; } = info;
        public IEnumerable<string> SourceList { get; set; } = [];
        public IEnumerable<string> DestinationList { get; set; } = [];
        public IEnumerable<string> MissingFiles { get; set; } = [];

        /// <summary>
        /// Set the Workflow IInfo Config property, accepts optional command line <paramref name="args"/> to
        /// specify a custom json config file name to load, otherwise it uses the <paramref name="defaultConfig"/>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="defaultConfig"></param>
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

        /// <summary>
        /// Read and Deserialize the specified Json file into the IInfo Config property
        /// </summary>
        /// <returns></returns>
        public async Task LoadConfig()
        {
            ConfigInfo = await GetInfoFromJson(Config);
            Console.WriteLine("Config found");
        }

        /// <summary>
        /// Read and store the names of all the compressed files in the source folder
        /// </summary>
        public void LoadCompressedSource()
        {
            SourceList = GetFileList(ConfigInfo);
            Console.WriteLine("Compressed Files Found");
        }

        /// <summary>
        /// Print to the console the list of compressed files
        /// </summary>
        /// <returns></returns>
        public async Task DisplaySource()
            => await PrintFileList(SourceList, ConfigInfo);

        /// <summary>
        /// Read and store the names of all the files in the destination folder
        /// </summary>
        public void LoadDestination()
        {

            DestinationList = DataProcessing.GetDirectoryFileList(ConfigInfo);
            Console.WriteLine("Destination Found");
        }

        /// <summary>
        /// Compare and store the names of the files in the compressed list that do not exist in the destination list
        /// </summary>
        public void IdentifyMissingFiles()
            => MissingFiles = DataProcessing.GetMissingSourceFiles(SourceList, DestinationList);

        /// <summary>
        /// Print to the console the list of missing files
        /// </summary>
        /// <returns></returns>
        public async Task DisplayMissingFiles()
            => await PrintFileList(MissingFiles, ConfigInfo, "missing");

        /// <summary>
        /// Export the list of missing files to the specified text file
        /// </summary>
        /// <returns></returns>
        public async Task ExportMissingFiles()
            => await ExportToFile(MissingFiles, ConfigInfo);

        /// <summary>
        /// Read and Deserialize the Json into the ConfigInfo property
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static async Task<IInfo> GetInfoFromJson(string configName)
        {
            var readData = await DataProcessing.ReadPathInfo(configName);
            var ConfigInfo = DataProcessing.GetAsInfo(readData);
            return ConfigInfo;
        }

        /// <summary>
        /// Read and store the names of all the compressed files in the source folder
        /// </summary>
        /// <param name="ConfigInfo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IEnumerable<string> GetFileList(IInfo ConfigInfo)
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
            var fileList = DataProcessing.GetCompressedFileList(ConfigInfo);
            return fileList;
        }

        /// <summary>
        /// if <paramref name="type"/> == "source" and <paramref name="info"/>.Verbose == true, 
        /// include compressed file internal files as part of the output
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="info"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static async Task PrintFileList(IEnumerable<string> fileList, IInfo info, string type = source)
        {
            await Task.Run(() =>
            {
                foreach (var file in fileList)
                {
                    var output = new List<string>
                    {
                        file
                    };

                    if (info.Verbose && type == source)
                    {
                        var fileContents = DataProcessing.GetCompressedFileContent(file);
                        foreach (var contents in fileContents)
                        {
                            output.Add($"{file}\t{contents}");
                        }
                        output.Add(Environment.NewLine);
                    }
                    foreach (var row in output.OrderBy(x => x))
                    {
                        Console.WriteLine(row);
                    }
                };
            });
        }

        /// <summary>
        /// Write the collection of file names to the specified file
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="info"></param>
        /// <returns></returns>
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
