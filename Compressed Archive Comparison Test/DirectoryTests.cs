using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class DirectoryTests
	{
		[TestMethod]
		public async Task A_GetDirectoryFileList_Not_Null()
		{
			var result = await DataProcessing.GetDirectoryFileList(TestData.TestDirInfo);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task A_GetDirectoryFileList_Content_Verified()
		{
			var result = await DataProcessing.GetDirectoryFileList(TestData.TestDirInfo);
			var expectedResult = TestData.FullPathResultList;
			Utilities.AssertAreEqual(result, expectedResult);
		}



		[TestMethod]
		public async Task A_GetDirectoryFileList_Empty_Not_Null()
		{
			var result = await DataProcessing.GetDirectoryFileList(TestData.EmptyFolderInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public async Task A_GetDirectoryFileList_Empty_Zero_Found()
		{
			var result = await DataProcessing.GetDirectoryFileList(TestData.EmptyFolderInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public async Task A_GetDirectoryFileList_Invalid_Not_Null()
		{
			var result = await DataProcessing.GetDirectoryFileList(TestData.BadFolderInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void B_FullPathToRelativeTextReplacement_Valid_Result()
		{
			var result = DataProcessing.FullPathToRelativeTextReplacement("Source\\TestDir1\\TestFile1.txt", TestData.SourceToRemove);
			var expectedResult = "TestDir1\\TestFile1.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void B_FullPathToRelativeTextReplacement_Valid__Result()
		{
			var result = DataProcessing.FullPathToRelativeTextReplacement("Source\\TestDir1\\TestFile1.txt", TestData.SourceToRemove, "Destination\\");
			var expectedResult = "Destination\\TestDir1\\TestFile1.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void B_FullPathToRelative_Valid_Result()
		{
			var result = DataProcessing.FullPathToRelative(TestData.ValidSourceList, TestData.SourceToRemove).ToList();
			var expectedResult = TestData.ExpectedResultList;
			Utilities.AssertAreEqual(result, expectedResult);
		}

		[TestMethod]
		public void B_FullPathToRelative_Valid_NoChange()
		{
			var result = DataProcessing.FullPathToRelative(TestData.ValidSourceList, "Nothing");
			var expectedResult = TestData.ValidSourceList;
			Utilities.AssertAreEqual(result, expectedResult);
		}

		[TestMethod]
		public void C_AddPathtoValue_Valid_Result()
		{
			var result = DataProcessing.AddPathToValue(TestData.ExpectedResultList, TestData.PathToAdd, "\\").ToList();
			var expectedResult = TestData.RelativePathResultList;
			Utilities.AssertAreEqual(result, expectedResult);
		}

		[TestMethod]
		public async Task D_GetMissingSourceFiles_Valid_Some_Exist_Result()
		{
			var result = await DataProcessing.GetMissingSourceFiles(TestData.FullPathSourcetList, TestData.FullPathResultList);
			var expectedResult = TestData.FullPathMissingList;
			Utilities.AssertAreEqual(result, expectedResult);
		}

		[TestMethod]
		public void E_NormalizeFileName_Valid_Full_Name()
		{
			var result = DataProcessing.NormalizeFileName(TestData.ValidFolderInfo.ExportFileName);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.FullPathFolderInfo.ExportFileName, result);
		}

		[TestMethod]
		public void E_NormalizeFileName_Empty_Name()
		{

			var result = DataProcessing.NormalizeFileName(TestData.EmptyFolderInfo.ExportFileName);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.NormalizedEmptyFileName, result);
		}

		[TestMethod]
		public void E_NormalizeFileName_Relative_Path_Name()
		{

			var result = DataProcessing.NormalizeFileName("SourceDir\\MissingFilesFound.txt");

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.NormalizedRelativeFileName, result);
		}

		[TestMethod]
		public void E_NormalizeFileName_Missing_File_Extension()
		{

			var result = DataProcessing.NormalizeFileName("MissingFilesFound");
			var expectedResult = TestData.FullPathFolderInfo.ExportFileName;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void E_NormalizeFileName_Bad_File_Extension()
		{
			var result = DataProcessing.NormalizeFileName("BadFileExtension.bat");

			Assert.IsNotNull(result);
			Assert.AreEqual("", result);
		}

		[TestMethod]
		public void F_OnlyFiles_Valid()
		{
			var result = DataProcessing.OnlyFiles(TestData.SourceCompressedFullList);
			var expectedResult = TestData.ExpectedSourceCompressedOnlyFilesList;
			Utilities.AssertAreEqual(result, expectedResult);
		}

		[TestMethod]
		public void H_GetFileName_Valid()
		{

			var result = DataProcessing.GetFileName(TestData.FullPathFolderInfo.CompressedSource + "\\TestFile.txt");
			var expectedResult = "TestFile.txt";
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void H_GetFileName_No_Change_Valid()
		{

			var result = DataProcessing.GetFileName("TestFile.txt");
			var expectedResult = "TestFile.txt";
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void H_GetFileName_No_File_Empty()
		{

			var result = DataProcessing.GetFileName(TestData.FullPathFolderInfo.CompressedSource + "\\");
			var expectedResult = "";
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void H_RemoveFoundFiles_Valid()
		{
			var result = DataProcessing.RemoveFoundFiles(TestData.FullPathFolderInfo, TestData.FullPathResultList, TestData.FullPathMissingList).ToList();
			var expectedResult = TestData.FullPathResultList;
			Utilities.AssertAreEqual(result, expectedResult);
		}

		[TestMethod]
		public void I_FilterDestination_Valid()
		{
			var result = DataProcessing.FilterDestination(TestData.DestinationFullList, TestData.FilterFolder).ToList();
			var expectedResult = TestData.DestinationFilteredList;
			Utilities.AssertAreEqual(result, expectedResult);
		}

		[TestMethod]
		public void I_FilterDestination_No_Results_Valid()
		{
			var result = DataProcessing.FilterDestination(TestData.FullPathResultList, TestData.FilterFolderNoResults).ToList();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}
	}
}
