namespace CompressedArchiveComparison.Config
{
    public class ConfigurationInfo : IInfo
    {
        public string CompressedSource { get; set; } = "";
        public string DeployDestination { get; set; } = "";
        public string ExclusionsFileName { get; set; } = "Exclusions.txt";
        public string ExportFileName { get; set; } = "MissingFilesFound.txt";
        public bool Verbose { get; set; } = false;
    }
}
