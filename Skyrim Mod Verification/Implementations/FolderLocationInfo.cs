
namespace SkyrimModVerification
{
    public class FolderLocationInfo : IInfo
	{
		public FolderLocationInfo()
		{
			CompressedSource = "";
			DeployDestination = "";
		}

		public string CompressedSource { get; set; }
		public string DeployDestination { get; set; }
	}
}
