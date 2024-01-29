using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class DirectoryTests
	{
		[TestMethod]
		public async Task A_GetDirectoryFileList_Not_Null()
		{
			var testInfo = TestData.TestDirInfo;
			var result = await DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(6, resultCount);
		}

		[TestMethod]
		public async Task A_GetDirectoryFileList_Count_Verified()
		{
			var testInfo = TestData.TestDirInfo;
			var result = await DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(6, resultCount);
		}

		[TestMethod]
		public async Task A_GetDirectoryFileList_Empty_Not_Null()
		{
			var testInfo = TestData.EmptyFolderInfo;
			var result = await DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task A_GetDirectoryFileList_Empty_Zero_Found()
		{
			var testInfo = TestData.EmptyFolderInfo;
			var result = await DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public async Task A_GetDirectoryFileList_Invalid_Not_Null()
		{
			var testInfo = TestData.EmptyFolderInfo;
			var result = await DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task A_GetDirectoryFileList_Invalid_Zero_Found()
		{
			var testInfo = TestData.EmptyFolderInfo;
			var result = await DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void B_FullPathToRelative_Valid_Result()
		{
			var result = DataProcessing.FullPathToRelative(TestData.ValidSourceList, TestData.SourceToRemove).ToList();
			var expectedResult = TestData.ExpectedResultList;
			var actualResultsFlattened = result.FlattenToString();
			var expectedResultsFlattened = expectedResult.FlattenToString();

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.Count, result.Count);
			Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
		}

		[TestMethod]
		public void B_FullPathToRelative_Valid_NoChange()
		{
			var result = DataProcessing.FullPathToRelative(TestData.ValidSourceList, "Nothing");
			var expectedResult = TestData.ValidSourceList;
			var actualResultsFlattened = result.FlattenToString();
			var expectedResultsFlattened = expectedResult.FlattenToString();

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.Count, result.ToList().Count);
			Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
		}

		[TestMethod]
		public void B_RelativeToFullPath_Valid_Result()
		{
			var result = DataProcessing.RelativeToFullPath(TestData.ExpectedResultList, TestData.PathToAdd, "\\").ToList();
			var expectedResult = TestData.RelativePathResultList;
			var actualResultsFlattened = result.FlattenToString();
			var expectedResultsFlattened = expectedResult.FlattenToString();

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.Count, result.Count);
			Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
		}

		[TestMethod]
		public async Task C_GetMissingSourceFiles_Valid_Result()
		{
			var result = await DataProcessing.GetMissingSourceFiles(TestData.FullPathFolderInfo, TestData.FullPathSourcetList, TestData.FullPathResultList);
			var expectedResult = TestData.FullPathMissingList;
			var actualResultsFlattened = result.FlattenToString();
			var expectedResultsFlattened = expectedResult.FlattenToString();

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.Count, result.ToList().Count);
			Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
		}

		[TestMethod]
		public void D_NormalizeFileName_Valid_Full_Name()
		{
			var result = DataProcessing.NormalizeFileName(TestData.ValidFolderInfo.ExportFileName);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ValidFolderInfo.ExportFileName, result);
		}

		[TestMethod]
		public void D_NormalizeFileName_Empty_Name()
		{

			var result = DataProcessing.NormalizeFileName(TestData.EmptyFolderInfo.ExportFileName);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.NormalizedEmptyFileName, result);
		}

		[TestMethod]
		public void D_NormalizeFileName_Relative_Path_Name()
		{

			var result = DataProcessing.NormalizeFileName("TestDir1\\MissingFilesFound.txt");

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.NormalizedRelativeFileName, result);
		}

		[TestMethod]
		public void D_NormalizeFileName_Missing_File_Extension()
		{

			var result = DataProcessing.NormalizeFileName(TestData.BadFolderInfo.ExportFileName);
			var expectedResult = TestData.BadFolderInfo.ExportFileName + ".txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void D_NormalizeFileName_Bad_File_Extension()
		{
			var result = DataProcessing.NormalizeFileName("BadFileExtension.bat");

			Assert.IsNotNull(result);
			Assert.AreEqual("", result);
		}

		[TestMethod]
		public void E_OnlyFiles_Valid()
		{
			var result = DataProcessing.OnlyFiles(TestData.EmptyFolderInfo, TestData.SourceCompressedFullList);
			var expectedResult = TestData.ExpectedSourceCompressedOnlyFilesList.FlattenToString();
			var resultFlattened = result.FlattenToString();

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, resultFlattened);
		}
	}
}
