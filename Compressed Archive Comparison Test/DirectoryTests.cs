using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class DirectoryTests
	{
		[TestMethod]
		public void A_GetDirectoryFileList_Not_Null()
		{
			var result = DataProcessing.GetDirectoryFileList(TestData.TestDirInfo);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void A_GetDirectoryFileList_Content_Verified()
		{
			var result = DataProcessing.GetDirectoryFileList(TestData.TestDirInfo).OrderBy(x => x);
			var expectedResult = TestData.FullPathResultList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public void A_GetDirectoryFileList_Empty_Zero_Found()
		{
			var result = DataProcessing.GetDirectoryFileList(TestData.EmptyFolderInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void A_GetDirectoryFileList_Invalid_Not_Null()
		{
			var result = DataProcessing.GetDirectoryFileList(TestData.BadFolderInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void C_AddPathtoValue_Correct_Value()
		{
			var result = DataProcessing.AddPathToValue(TestData.ExpectedResultList, TestData.PathToAdd, "\\");
			var expectedResult = TestData.RelativePathResultList;

			Assert.IsNotNull(result);
			result = result.OrderBy(x => x);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public void D_GetMissingSourceFiles_Correct_Value()
		{
			var result = DataProcessing.GetMissingSourceFiles(TestData.FullPathSourcetList, TestData.FullPathResultList);
			var expectedResult = TestData.FullPathMissingList;

			Assert.IsNotNull(result);
			result = result.OrderBy(x => x);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public async Task D_DetermineMissingFiles_Correct_Value()
		{
			var result = await DataProcessing.DetermineMissingFiles(TestData.FullPathSourcetList[0], TestData.FullPathResultList);
			var expectedResult = TestData.FullPathMissingList.Take(3).ToList();

			Assert.IsNotNull(result);
			result = result.OrderBy(x => x);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public void E_NormalizeFileName_Correct_Value()
		{
			var result = DataProcessing.NormalizeFileName(TestData.ValidFolderInfo.ExportFileName);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.FullPathFolderInfo.ExportFileName, result);
		}

		[TestMethod]
		public void E_NormalizeFileName_Empty_Name()
		{
			var result = DataProcessing.NormalizeFileName(TestData.EmptyFolderInfo.ExportFileName);
			var expectedResult = TestData.NormalizedEmptyFileName;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void E_NormalizeFileName_Relative_Path_Name()
		{
			var result = DataProcessing.NormalizeFileName("SourceDir\\MissingFilesFound.txt");
			var expectedResult = TestData.NormalizedRelativeFileName;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
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
		public void F_OnlyFiles_Correct_Value()
		{
			var result = DataProcessing.OnlyFiles(TestData.SourceCompressedFullList);
			var expectedResult = TestData.ExpectedSourceCompressedOnlyFilesList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public void H_GetFileName_Correct_Value()
		{

			var result = DataProcessing.GetFileName(TestData.FullPathFolderInfo.CompressedSource + "\\TestFile.txt");
			var expectedResult = "TestFile.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void H_GetFileName_No_Change_Correct_Value()
		{
			var result = DataProcessing.GetFileName("TestFile.txt");
			var expectedResult = "TestFile.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void H_GetFileName_No_File_Empty()
		{
			var result = DataProcessing.GetFileName(TestData.FullPathFolderInfo.CompressedSource + "\\");
			var expectedResult = "";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void I_GetFolderName_Correct_Value()
		{
			var result = DataProcessing.GetFolderName("TestFile.txt");
			var expectedResult = "TestFile";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void I_GetFolderName_No_Change_Correct_Value()
		{
			var result = DataProcessing.GetFolderName("TestFile");
			var expectedResult = "TestFile";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void I_GetFolderName_No_File_Empty()
		{
			var result = DataProcessing.GetFolderName("");
			var expectedResult = "";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public async Task J_FilterDestination_Correct_Value()
		{
			var result = await DataProcessing.FilterDestination(TestData.DestinationFullList, TestData.FilterFolder);
			var expectedResult = TestData.DestinationFilteredList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public async Task J_FilterDestination_No_Results_Correct_Value()
		{
			var result = await DataProcessing.FilterDestination(TestData.FullPathResultList, TestData.FilterFolderNoResults);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count());
		}
	}
}
