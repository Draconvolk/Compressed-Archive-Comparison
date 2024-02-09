using CompressedArchiveComparison.Components;
using CompressedArchiveComparison.Config;
using CompressedArchiveComparison.Exceptions;

namespace CompressedArchiveComparison.Workflow
{
    public class WorkflowActions(IExceptionList exceptionList, DataProcessing dataProcessing, IInfo info) : IWorkflowActions
    {
        private static readonly string WFA = "WorkflowActions";
        private const string source = "source";

        public string Config { get; set; } = "Config.json";
        public IInfo ConfigInfo { get; set; } = info;
        public IEnumerable<string> SourceList { get; set; } = [];
        public IEnumerable<string> ExclusionList { get; set; } = [];
        public IEnumerable<string> DestinationList { get; set; } = [];
        public IEnumerable<string> MissingFiles { get; set; } = [];

        /// <summary>
        /// Set the <see cref="Config"/> property, accepts optional command line <paramref name="args"/> to
        /// specify a custom JSON config filename to load, otherwise it uses the <paramref name="defaultConfig"/>
        /// </summary>
        /// <param name="args"></param>
        /// <param name="defaultConfig"></param>
        public void SetConfig(string[] args, string defaultConfig)
        {
            var configName = defaultConfig;
            try
            {
                if (args.Length != 0)
                {
                    configName = args[0];
                    configName = string.IsNullOrWhiteSpace(configName?.Trim()) ? defaultConfig : configName.Trim();
                }
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, "Error loading the name of the config file", $"{WFA}\\SetConfig", [.. args, defaultConfig]);
            }
            Config = configName;
            Console.WriteLine($"Config name set to [{Config}]");
        }

        /// <summary>
        /// Read and Deserialize the specified Json file into the IInfo Config property
        /// </summary>
        /// <returns></returns>
        public async Task LoadConfig()
        {
            try
            {
                ConfigInfo = await GetInfoFromJson(Config);
                Console.WriteLine("Config found");
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, "Error loading the config file", $"{WFA}\\LoadConfig", [Config]);
            }
        }

        /// <summary>
        /// Read and store the names of all the compressed files in the source folder
        /// </summary>
        public async Task LoadCompressedSource()
        {
            try
            {
                var exclusionTask = GetFileExclusions(ConfigInfo);
                var fullSource = GetFileList(ConfigInfo);
                ExclusionList = await exclusionTask;
                SourceList = FilterSourceList(fullSource, ExclusionList);

                Console.WriteLine("Compressed Files Found");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, "Error loading the compressed source", $"{WFA}\\LoadCompressedSource");
            }
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
            try
            {
                DestinationList = dataProcessing.GetDirectoryFileList(ConfigInfo);
                Console.WriteLine("Destination Found");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, "Error loading the destination files", $"{WFA}\\LoadDestination");
            }
        }

        /// <summary>
        /// Compare and store the names of the files in the compressed list that do not exist in the destination list
        /// </summary>
        public void IdentifyMissingFiles()
            => MissingFiles = dataProcessing.GetMissingSourceFiles(SourceList, DestinationList);

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
        /// Returns a collection of values from sourceList that do not exist in exclusionList
        /// </summary>
        /// <param name="sourceList"></param>
        /// <param name="exclusionList"></param>
        /// <returns></returns>
        public IEnumerable<string> FilterSourceList(IEnumerable<string> sourceList, IEnumerable<string> exclusionList)
            => dataProcessing.FilterSourceList(sourceList, exclusionList);

        /// <summary>
        /// Read the data from the exclusion file listed in the configInfo and return it as a collection
        /// </summary>
        /// <param name="configInfo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetFileExclusions(IInfo configInfo)
        {
            try
            {
                var exclusionText = await dataProcessing.GetExclusionFileText(configInfo);
                var exclusionList = dataProcessing.ParseExclusionFileText(exclusionText, dataProcessing.DefaultSeparator);
                return exclusionList;
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, "Error loading the exclusion file content", $"{WFA}\\GetFileExclusions", [configInfo.ExclusionsFileName]);
                return [];
            }
        }

        /// <summary>
        /// Read and Deserialize the Json into the configInfo property
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public async Task<IInfo> GetInfoFromJson(string configName)
        {
            try
            {
                var readData = await dataProcessing.ReadPathInfo(configName);
                var ConfigInfo = dataProcessing.GetAsInfo(readData);
                return ConfigInfo;
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, "Error loading the JSON data", $"{WFA}\\GetInfoFromJson", [configName]);
                throw;
            }
        }

        /// <summary>
        /// Read and store the names of all the compressed files in the source folder
        /// </summary>
        /// <param name="configInfo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<string> GetFileList(IInfo configInfo)
        {
            try
            {
                if (!dataProcessing.IsValidInfo(configInfo))
                {
                    Console.WriteLine("Invalid Source and Destination data in Config file. Please specify valid paths and try again.");
                    Console.WriteLine();
                    var ex = new Exception("Invalid data in config file.");
                    exceptionList.Add(ex, "Error loading the name of the config file", $"{WFA}\\GetFileList",
                        [configInfo.CompressedSource,
                            configInfo.DeployDestination,
                            configInfo.ExclusionsFileName,
                            configInfo.ExportFileName]);
                    return [];
                }

                Console.WriteLine();
                Console.WriteLine($"Src: {configInfo.CompressedSource}");
                Console.WriteLine($"Dest: {configInfo.DeployDestination}");
                Console.WriteLine();
                var fileList = dataProcessing.GetCompressedFileList(configInfo);
                return fileList;
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, "Error loading the source file list", $"{WFA}\\GetFileList", [configInfo.CompressedSource]);
                return [];
            }
        }

        /// <summary>
        /// if <paramref name="type"/> == "source" and <paramref name="info"/>.Verbose == true, 
        /// include compressed file internal files as part of the output
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="info"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task PrintFileList(IEnumerable<string> fileList, IInfo info, string type = source)
        {
            await Task.Run(() =>
            {
                Console.WriteLine($"Displaying {type} List");
                var previousCompressed = "";

                try
                {
                    foreach (var file in fileList)
                    {
                        var output = new List<string>
                    {
                        file
                    };
                        if (type != source)
                        {
                            var compressedFileName = file[..file.IndexOf('|')];
                            if (previousCompressed != compressedFileName)
                            {
                                previousCompressed = compressedFileName;
                                output.Add(Environment.NewLine);
                            }
                        }
                        if (info.Verbose && type == source)
                        {
                            var fileContents = dataProcessing.GetCompressedFileContent(file);
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
                }
                catch (Exception ex)
                {
                    exceptionList.Add(ex, $"Error displaying {source} file list ", $"{WFA}\\PrintFileList");
                }
                Console.WriteLine();
            });
        }

        /// <summary>
        /// Write the collection of file names to the specified file
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task ExportToFile(IEnumerable<string> fileList, IInfo info)
        {
            try
            {
                if (await dataProcessing.WriteToFile(fileList, info.ExportFileName))
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
            catch (Exception ex)
            {
                exceptionList.Add(ex, $"Error exporting list to the file {info.ExportFileName}", $"{WFA}\\ExportToFile", [info.ExportFileName]);
            }
        }
    }
}
