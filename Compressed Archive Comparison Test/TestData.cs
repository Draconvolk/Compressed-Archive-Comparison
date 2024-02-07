using CompressedArchiveComparison.Config;

namespace CompressedArchiveComparisonTests
{
	public static class TestData
	{
		public static readonly string TestInfoJson = "{\r\n\t\"CompressedSource\": \"SourceDir\",\r\n\t\"DeployDestination\": \"DestinationDir\",\r\n\t\"ExclusionsFileName\": \"TestExclusions.txt\",\r\n\t\"ExportFileName\": \"MissingFilesFound.txt\",\r\n\t\"Verbose\": true\r\n}\r\n";
		public static readonly IInfo ValidFolderInfo = new ConfigurationInfo()
		{
			CompressedSource = "SourceDir",
			DeployDestination = "DestinationDir",
			ExclusionsFileName = "TestExclusions.txt",
			ExportFileName = "MissingFilesFound.txt",
			Verbose = true
		};
		public static readonly IInfo EmptyFolderInfo = new ConfigurationInfo()
		{
			CompressedSource = "",
			DeployDestination = "",
			ExclusionsFileName = "",
			ExportFileName = "",
			Verbose = false
		};
		public static readonly IInfo BadFolderInfo = new ConfigurationInfo()
		{
			CompressedSource = "Unknown Folder",
			DeployDestination = "Second Unknown Folder",
			ExclusionsFileName = "BadExclusions.bat",
			ExportFileName = "MissingFilesFound.bat",
			Verbose = false
		};
		public static readonly IInfo DefaultInfo_Result = new ConfigurationInfo()
		{
			CompressedSource = "",
			DeployDestination = "",
			ExclusionsFileName = "Exclusions.txt",
			ExportFileName = "MissingFilesFound.txt",
			Verbose = false
		};
		public static readonly List<string> ExclusionFileList = [
			"TestSevenZip.7z",
			"TestZip.zip"
		];
		public static readonly List<string> FilteredSourceList = [
			"SourceDir\\TestDir1.7z",
			"SourceDir\\TestDir2.7z",
			"SourceDir\\TestRar.rar",
		];		
		public static readonly string EmptyExclusions = "TestEmptyExclusions.txt";
		public static readonly string ValidPath = Environment.CurrentDirectory;
		public static readonly string ValidSourceDir = Path.Combine(ValidPath, "SourceDir");
		public static readonly string ValidDestinationDir = Path.Combine(ValidPath, "DestinationDir");
		public static readonly string ValidCompressedFile = Path.Combine(ValidSourceDir, "TestZip.zip");
		public static readonly string ValidCompressedFileZip = Path.Combine(ValidSourceDir, "TestZip.zip");
		public static readonly string ValidCompressedFile7z = Path.Combine(ValidSourceDir, "TestSevenZip.7z");
		public static readonly string ValidCompressedFileRar = Path.Combine(ValidSourceDir, "TestRar.rar");
		public static readonly string InvalidCompressed7z = Path.Combine(ValidSourceDir, "badtest.7z");
		public static readonly string InvalidCompressedRar = Path.Combine(ValidSourceDir, "badtest.rar");
		public static readonly string InvalidCompressedZip = Path.Combine(ValidSourceDir, "badtest.zip");
		public static readonly IInfo TestDirInfo = new ConfigurationInfo()
		{
			CompressedSource = ValidSourceDir,
			DeployDestination = ValidDestinationDir,
			ExclusionsFileName = "TestExclusions.txt",
			ExportFileName = "MissingFilesFound.txt",
			Verbose = true
		};
		public static readonly List<string> RelativePathList = [
			"TestDir1\\NestedDir1\\TestFile3.txt",
			"TestDir1\\NestedDir1\\TestFile4.txt",
			"TestDir1\\NestedDir1\\TestFile5.txt",
			"TestDir1\\NestedDir2\\TestFile6.txt",
			"TestDir1\\TestFile1.txt",
			"TestDir1\\TestFile2.txt"
		];
		public static readonly string SourceToRemove = "Source\\";
		public static readonly string PathToAdd = $"{ValidDestinationDir}\\TestDir1.7z";
		public static readonly List<string> RelativePathList_Result = [
			$"{PathToAdd}\\TestDir1\\NestedDir1\\TestFile3.txt",
			$"{PathToAdd}\\TestDir1\\NestedDir1\\TestFile4.txt",
			$"{PathToAdd}\\TestDir1\\NestedDir1\\TestFile5.txt",
			$"{PathToAdd}\\TestDir1\\NestedDir2\\TestFile6.txt",
			$"{PathToAdd}\\TestDir1\\TestFile1.txt",
			$"{PathToAdd}\\TestDir1\\TestFile2.txt"
		];
		public static readonly List<string> FullPathList_Result = [
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile3.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile4.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile5.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir2\\TestFile6.txt",
			$"{ValidDestinationDir}\\TestDir1\\TestFile1.txt",
			$"{ValidDestinationDir}\\TestDir1\\TestFile2.txt",
			$"{ValidDestinationDir}\\TestDir2\\NestedDir1\\TestFile2.txt",
			$"{ValidDestinationDir}\\TestDir2\\NestedDir3\\TestFile4.txt",
			$"{ValidDestinationDir}\\TestDir2\\TestFile1.txt"
		];
		public static readonly List<string> FullPathSourcetList = [
			$"{ValidSourceDir}\\TestDir1.7z",
			$"{ValidSourceDir}\\TestSevenZip.7z"
		];
		public static readonly IInfo FullPathFolderInfo = new ConfigurationInfo()
		{
			CompressedSource = ValidSourceDir,
			DeployDestination = ValidDestinationDir,
			ExclusionsFileName = $"{ValidPath}\\TestExclusions.txt",
			ExportFileName = $"{ValidPath}\\MissingFilesFound.txt",
			Verbose = false
		};
		public static readonly List<string> FullPathMissingList = [
			$"{ValidSourceDir}\\TestDir1.7z|NestedDir1\\TestFile8.txt",
			$"{ValidSourceDir}\\TestDir1.7z|NestedDir2\\TestFile9.txt",
			$"{ValidSourceDir}\\TestDir1.7z|TestFile7.txt",
			$"{ValidSourceDir}\\TestSevenZip.7z|TestFile1.txt",
			$"{ValidSourceDir}\\TestSevenZip.7z|TestFile2.txt"
		];
		public static readonly string NormalizedEmptyFileName = Path.Combine(ValidPath, "MissingFilesFound.txt");
		public static readonly string NormalizedRelativeFileName = Path.Combine(ValidSourceDir, "MissingFilesFound.txt");
		public static readonly List<string> SourceCompressedFullList = [
			"TestDir1",
			"TestDir1\\NestedDir1",
			"TestDir1\\NestedDir1\\TestFile3.txt",
			"TestDir1\\NestedDir1\\TestFile4.txt",
			"TestDir1\\NestedDir1\\TestFile5.txt",
			"TestDir1\\NestedDir1\\TestFile8.txt",
			"TestDir1\\NestedDir2",
			"TestDir1\\NestedDir2\\TestFile6.txt",
			"TestDir1\\NestedDir2\\TestFile9.txt",
			"TestDir1\\TestFile1.txt",
			"TestDir1\\TestFile2.txt",
			"TestDir1\\TestFile7.txt"
		];
		public static readonly List<string> SourceCompressedOnlyFilesList_Result = [
			"TestDir1\\NestedDir1\\TestFile3.txt",
			"TestDir1\\NestedDir1\\TestFile4.txt",
			"TestDir1\\NestedDir1\\TestFile5.txt",
			"TestDir1\\NestedDir1\\TestFile8.txt",
			"TestDir1\\NestedDir2\\TestFile6.txt",
			"TestDir1\\NestedDir2\\TestFile9.txt",
			"TestDir1\\TestFile1.txt",
			"TestDir1\\TestFile2.txt",
			"TestDir1\\TestFile7.txt"
		];
		public static readonly List<string> SourceCompressedRobustFullList = [
			"Test.Dir1","Test.Dir1\\Nested_Dir1",
			"Test.Dir1\\Nested_Dir1\\Test_File3.toml",
			"Test.Dir1\\Nested_Dir1\\Test - File4.nif",
			"Test.Dir1\\Nested_Dir1\\Test.File.7.nif",
			"Test.Dir1\\Nested_Dir1\\TestFile8.nif",
			"Test.Dir1\\Nested . Dir2",
			"Test.Dir1\\Nested . Dir2\\Test--File6.esp",
			"Test.Dir1\\Nested . Dir2\\Test _ File  - 6.bsa",
			"Test.Dir1\\Nested- Dir.3",
			"Test.Dir1\\Nested- Dir.3\\TestFile1.exe",
			"Test.Dir1\\Nested- Dir.3\\TestFile2.dll",
			"Test.Dir1\\Only Folder .Test - Exception",
			"Test.Dir1\\Only Folder .Test - Exception\\Deepest-Folder",
			"Test.Dir1\\Test_File1.txt",
			"Test.Dir1\\Test File2.bat",
			"Test.Dir1\\Test.File7.esp"
		];
		public static readonly List<string> SourceCompressedOnlyFilesRobustList_Result = [			
			"Test.Dir1\\Nested_Dir1\\Test_File3.toml",
			"Test.Dir1\\Nested_Dir1\\Test - File4.nif",
			"Test.Dir1\\Nested_Dir1\\Test.File.7.nif",
			"Test.Dir1\\Nested_Dir1\\TestFile8.nif",
			"Test.Dir1\\Nested . Dir2\\Test--File6.esp",
			"Test.Dir1\\Nested . Dir2\\Test _ File  - 6.bsa",
			"Test.Dir1\\Nested- Dir.3\\TestFile1.exe",
			"Test.Dir1\\Nested- Dir.3\\TestFile2.dll",
			"Test.Dir1\\Only Folder .Test - Exception\\Deepest-Folder",
			"Test.Dir1\\Test_File1.txt",
			"Test.Dir1\\Test File2.bat",
			"Test.Dir1\\Test.File7.esp"
		];
		public static readonly List<string> DestinationFullList = [
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile3.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile4.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile5.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir2\\TestFile6.txt",
			$"{ValidDestinationDir}\\TestDir1\\TestFile1.txt",
			$"{ValidDestinationDir}\\TestDir1\\TestFile2.txt",
			$"{ValidDestinationDir}\\TestDir2\\NestedDir1\\TestFile2.txt",
			$"{ValidDestinationDir}\\TestDir2\\NestedDir3\\TestFile4.txt",
			$"{ValidDestinationDir}\\TestDir2\\TestFile1.txt",
			$"{ValidDestinationDir}\\TestDir3\\NestedDir3\\TestFile5.txt",
			$"{ValidDestinationDir}\\TestDir3\\TestFile2.txt"
		];
		public static readonly string FilterFolder = "TestDir1";
		public static readonly string FilterFolderNoResults = "TestDir4";
		public static readonly List<string> DestinationFilteredList = [
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile3.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile4.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir1\\TestFile5.txt",
			$"{ValidDestinationDir}\\TestDir1\\NestedDir2\\TestFile6.txt",
			$"{ValidDestinationDir}\\TestDir1\\TestFile1.txt",
			$"{ValidDestinationDir}\\TestDir1\\TestFile2.txt",
		];
		public static readonly List<string> DestinationFilteredMissingList = [
			"TestDir1\\NestedDir1\\TestFile8.txt",
			"TestDir1\\NestedDir2\\TestFile9.txt",
			"TestDir1\\TestFile7.txt"
		];
		public static readonly List<string> CompressedSourceUnFiltered_Result = [
			"SourceDir\\TestDir1.7z",
			"SourceDir\\TestDir2.7z",
			"SourceDir\\TestRar.rar",
			"SourceDir\\TestSevenZip.7z",
			"SourceDir\\TestZip.zip"
		];
		public static readonly List<string> LoadDestination_Result = [
			"DestinationDir\\TestDir1\\NestedDir1\\TestFile3.txt",
			"DestinationDir\\TestDir1\\NestedDir1\\TestFile4.txt",
			"DestinationDir\\TestDir1\\NestedDir1\\TestFile5.txt",
			"DestinationDir\\TestDir1\\NestedDir2\\TestFile6.txt",
			"DestinationDir\\TestDir1\\TestFile1.txt",
			"DestinationDir\\TestDir1\\TestFile2.txt",
			"DestinationDir\\TestDir2\\NestedDir1\\TestFile2.txt",
			"DestinationDir\\TestDir2\\NestedDir3\\TestFile4.txt",
			"DestinationDir\\TestDir2\\TestFile1.txt"
		];
		public static readonly List<string> IdentifyMissingFiles_Result = [
			"SourceDir\\TestDir1.7z|NestedDir1\\TestFile8.txt",
			"SourceDir\\TestDir1.7z|NestedDir2\\TestFile9.txt",
			"SourceDir\\TestDir1.7z|TestFile7.txt",
			"SourceDir\\TestDir2.7z|NestedDir1\\TestFile3.txt",
			"SourceDir\\TestDir2.7z|NestedDir1\\TestFile4.txt",
			"SourceDir\\TestDir2.7z|NestedDir1\\TestFile5.txt",
			"SourceDir\\TestDir2.7z|NestedDir1\\TestFile8.txt",
			"SourceDir\\TestDir2.7z|NestedDir2\\TestFile6.txt",
			"SourceDir\\TestDir2.7z|NestedDir2\\TestFile9.txt",			
			"SourceDir\\TestDir2.7z|TestFile7.txt",
			"SourceDir\\TestRar.rar|TestFile1.txt",
			"SourceDir\\TestRar.rar|TestFile2.txt"
			];
	}
}