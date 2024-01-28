namespace Skyrim_Mod_Verification
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
