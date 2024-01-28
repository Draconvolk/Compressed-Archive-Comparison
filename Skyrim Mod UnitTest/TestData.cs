using Skyrim_Mod_Verification;

namespace Skyrim_Mod_UnitTest
{
	public static class TestData
	{
		public static readonly string ConfigLocationsJson = "{\r\n\t\"CompressedSource\": \"C:\\\\Games\\\\Skyrm Downloads\\\\SkyrimSE\",\r\n\t\"DeployDestination\": \"C:\\\\Games\\\\Skyrim Mods\"\r\n}\r\n";
		public static readonly FolderLocationInfo ValidFolderInfo = new() { CompressedSource = @"C:\Games\Skyrm Downloads\SkyrimSE", DeployDestination = @"C:\Games\Skyrim Mods" };
		public static readonly FolderLocationInfo EmptyFolderInfo = new() { CompressedSource = "", DeployDestination = "" };
		public static readonly FolderLocationInfo BadFolderInfo = new() { CompressedSource = @"C:\Games\Unknown Folder", DeployDestination = @"C:\Games\Second Unknown Folder" };
	}
}