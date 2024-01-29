using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	public static class TestData
	{
		public static readonly string ConfigLocationsJson = "{\r\n\t\"CompressedSource\": \"C:\\\\Games\\\\Skyrim Downloads\\\\SkyrimSE\",\r\n\t\"DeployDestination\": \"C:\\\\Games\\\\Skyrim Mods\",\r\n\t\"ExportFileName\": \"C:\\\\Games\\\\MissingFilesFound.txt\",\r\n\t\"Verbose\": true\r\n}\r\n";
		public static readonly IInfo ValidFolderInfo = new FolderLocationInfo()
		{
			CompressedSource = @"C:\Games\Skyrim Downloads\SkyrimSE",
			DeployDestination = @"C:\Games\Skyrim Mods",
			ExportFileName = @"C:\Games\MissingFilesFound.txt",
			Verbose = true
		};
		public static readonly IInfo EmptyFolderInfo = new FolderLocationInfo()
		{
			CompressedSource = "",
			DeployDestination = "",
			ExportFileName = @"",
			Verbose = false
		};
		public static readonly IInfo BadFolderInfo = new FolderLocationInfo()
		{
			CompressedSource = @"C:\Games\Unknown Folder",
			DeployDestination = @"C:\Games\Second Unknown Folder",
			ExportFileName = @"C:\Games\MissingFilesFound",
			Verbose = false
		};
		public static readonly string ValidPath = Environment.CurrentDirectory;
		public static readonly string ValidCompressedFile = Path.Combine(ValidPath, "TestZip.zip");
		public static readonly string ValidCompressedFileZip = Path.Combine(ValidPath, "TestZip.zip");
		public static readonly string ValidCompressedFile7z = Path.Combine(ValidPath, "TestSevenZip.7z");
		public static readonly string ValidCompressedFileRar = Path.Combine(ValidPath, "TestRar.rar");
		public static readonly string InvalidCompressed7z = Path.Combine(ValidPath, "badtest.7z");
		public static readonly string InvalidCompressedRar = Path.Combine(ValidPath, "badtest.rar");
		public static readonly string InvalidCompressedZip = Path.Combine(ValidPath, "badtest.zip");
		public static readonly string ValidDir = Path.Combine(ValidPath, "TestDir1");
		public static readonly IInfo TestDirInfo = new FolderLocationInfo()
		{
			CompressedSource = ValidDir,
			DeployDestination = ValidDir,
			Verbose = true
		};
		public static readonly List<string> ValidSourceList = [
			"Source\\TestDir1\\TestFile1.txt",
			"Source\\TestDir1\\TestFile2.txt",
			"Source\\TestDir1\\NestedDir1\\TestFile3.txt",
			"Source\\TestDir1\\NestedDir1\\TestFile4.txt",
			"Source\\TestDir1\\NestedDir1\\TestFile5.txt",
			"Source\\TestDir1\\NestedDir2\\TestFile6.txt"
		];
		public static readonly List<string> ExpectedResultList = [
			"TestDir1\\TestFile1.txt",
			"TestDir1\\TestFile2.txt",
			"TestDir1\\NestedDir1\\TestFile3.txt",
			"TestDir1\\NestedDir1\\TestFile4.txt",
			"TestDir1\\NestedDir1\\TestFile5.txt",
			"TestDir1\\NestedDir2\\TestFile6.txt"
		];
		public static readonly string SourceToRemove = "Source\\";
		public static readonly string PathToAdd = $"{ValidDir}\\TestZip.zip";
		public static readonly List<string> RelativePathResultList = [
			$"{PathToAdd}\\TestDir1\\TestFile1.txt",
			$"{PathToAdd}\\TestDir1\\TestFile2.txt",
			$"{PathToAdd}\\TestDir1\\NestedDir1\\TestFile3.txt",
			$"{PathToAdd}\\TestDir1\\NestedDir1\\TestFile4.txt",
			$"{PathToAdd}\\TestDir1\\NestedDir1\\TestFile5.txt",
			$"{PathToAdd}\\TestDir1\\NestedDir2\\TestFile6.txt"
		];
		public static readonly List<string> FullPathResultList = [
			$"{ValidDir}TestDir1\\TestFile1.txt",
			$"{ValidDir}TestDir1\\TestFile2.txt",
			$"{ValidDir}TestDir1\\NestedDir1\\TestFile3.txt",
			$"{ValidDir}TestDir1\\NestedDir1\\TestFile4.txt",
			$"{ValidDir}TestDir1\\NestedDir1\\TestFile5.txt",
			$"{ValidDir}TestDir1\\NestedDir2\\TestFile6.txt"
		];
		public static readonly List<string> FullPathSourcetList = [
			$"{ValidPath}\\TestRar.rar",
			$"{ValidPath}\\TestSevenZip.7z"
		];
		public static readonly IInfo FullPathFolderInfo = new FolderLocationInfo()
		{
			CompressedSource = ValidPath,
			DeployDestination = ValidDir,
			ExportFileName = $"{ValidPath}\\MissingFilesFound.txt",
			Verbose = true
		};
		public static readonly List<string> FullPathMissingList = [
			$"{ValidPath}\\TestRar.rar\\TestFile1.txt",
			$"{ValidPath}\\TestRar.rar\\TestFile2.txt",
			$"{ValidPath}\\TestSevenZip.7z\\TestFile1.txt",
			$"{ValidPath}\\TestSevenZip.7z\\TestFile2.txt"
		];
		public static readonly string NormalizedEmptyFileName = Path.Combine(ValidPath, "MissingFilesFound.txt");
		public static readonly string NormalizedRelativeFileName = Path.Combine(ValidDir, "MissingFilesFound.txt");
	}
}