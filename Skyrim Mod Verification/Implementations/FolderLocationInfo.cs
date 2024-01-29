
namespace CompressedArchiveComparison
{
    public class FolderLocationInfo : IInfo
	{
		public FolderLocationInfo()
		{
			CompressedSource = "";
			DeployDestination = "";
			ExportFileName = "MissingFilesFound.txt";
			Verbose = false;
		}

		public string CompressedSource { get; set; }
		public string DeployDestination { get; set; }
		public string ExportFileName { get; set; }
		public bool Verbose { get; set; }
	}
}
