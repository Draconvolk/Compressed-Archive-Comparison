namespace CompressedArchiveComparison.Interfaces
{
    public interface IInfo
    {
        string CompressedSource { get; set; }
        string DeployDestination { get; set; }
        /// <summary>
        /// Valid file is of type txt
        /// </summary>
        string ExclusionsFileName { get; set; }
        /// <summary>
        /// Valid File extensions are of type txt or log
        /// </summary>
        string ExportFileName { get; set; }
        /// <summary>
        /// Toggle additional output
        /// </summary>
        bool Verbose { get; set; }
    }
}
