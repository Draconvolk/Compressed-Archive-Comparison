using CompressedArchiveComparison.Components;

namespace CompressedArchiveComparisonTests
{
    [TestClass]
    public class DirectoryTests
    {
        private readonly DataProcessing DProcessing = Utilities.GetInjectedObject<DataProcessing>() ?? throw new Exception("Failed to load DataProcessing");

        [TestMethod]
        public void A_GetDirectoryFileList_Not_Null()
        {
            var result = DProcessing.GetDirectoryFileList(TestData.TestDirInfo);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void A_GetDirectoryFileList_Content_Verified()
        {
            var result = DProcessing.GetDirectoryFileList(TestData.TestDirInfo).OrderBy(x => x);
            var expectedResult = TestData.FullPathList_Result;

            Assert.IsNotNull(result);
            Utilities.AssertAreEqual(expectedResult, result);
        }

        [TestMethod]
        public void A_GetDirectoryFileList_Empty_Zero_Found()
        {
            var result = DProcessing.GetDirectoryFileList(TestData.EmptyFolderInfo);
            var resultCount = result.Count();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, resultCount);
        }

        [TestMethod]
        public void A_GetDirectoryFileList_Invalid_Not_Null()
        {
            var result = DProcessing.GetDirectoryFileList(TestData.BadFolderInfo);
            var resultCount = result.Count();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, resultCount);
        }

        [TestMethod]
        public void C_AddPathtoValue_Correct_Value()
        {
            var result = DProcessing.AddPathToValue(TestData.RelativePathList, TestData.PathToAdd, "\\");
            var expectedResult = TestData.RelativePathList_Result;

            Assert.IsNotNull(result);
            result = result.OrderBy(x => x);
            Utilities.AssertAreEqual(expectedResult, result);
        }

        [TestMethod]
        public void D_GetMissingSourceFiles_Correct_Value()
        {
            var result = DProcessing.GetMissingSourceFiles(TestData.FullPathSourcetList, TestData.FullPathList_Result);
            var expectedResult = TestData.FullPathMissingList;

            Assert.IsNotNull(result);
            result = result.OrderBy(x => x);
            Utilities.AssertAreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task D_DetermineMissingFiles_Correct_Value()
        {
            var result = await DProcessing.DetermineMissingFiles(TestData.FullPathSourcetList[0], TestData.FullPathList_Result);
            var expectedResult = TestData.FullPathMissingList.Take(3).ToList();

            Assert.IsNotNull(result);
            result = result.OrderBy(x => x);
            Utilities.AssertAreEqual(expectedResult, result);
        }

        [TestMethod]
        public void E_NormalizeFileName_Correct_Value()
        {
            var result = DProcessing.NormalizeFileName(TestData.ValidFolderInfo.ExportFileName);

            Assert.IsNotNull(result);
            Assert.AreEqual(TestData.FullPathFolderInfo.ExportFileName, result);
        }

        [TestMethod]
        public void E_NormalizeFileName_Empty_Name()
        {
            var result = DProcessing.NormalizeFileName(TestData.EmptyFolderInfo.ExportFileName);
            var expectedResult = TestData.NormalizedEmptyFileName;

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void E_NormalizeFileName_Relative_Path_Name()
        {
            var result = DProcessing.NormalizeFileName("SourceDir\\MissingFilesFound.txt");
            var expectedResult = TestData.NormalizedRelativeFileName;

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void E_NormalizeFileName_Missing_File_Extension()
        {
            var result = DProcessing.NormalizeFileName("MissingFilesFound");
            var expectedResult = TestData.FullPathFolderInfo.ExportFileName;

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void E_NormalizeFileName_Bad_File_Extension()
        {
            var result = DProcessing.NormalizeFileName("BadFileExtension.bat");

            Assert.IsNotNull(result);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void F_OnlyFiles_Correct_Value()
        {
            var result = DProcessing.OnlyFiles(TestData.SourceCompressedFullList);
            var expectedResult = TestData.SourceCompressedOnlyFilesList_Result;

            Assert.IsNotNull(result);
            Utilities.AssertAreEqual(expectedResult, result);
        }

        [TestMethod]
        public void F_OnlyFiles_Robust_Correct_Value()
        {
            var result = DProcessing.OnlyFiles(TestData.SourceCompressedRobustFullList);
            var expectedResult = TestData.SourceCompressedOnlyFilesRobustList_Result;

            Assert.IsNotNull(result);
            Utilities.AssertAreEqual(expectedResult, result);
        }

        [TestMethod]
        public void G_FilterMissingFiles_Correct_Value()
        {
            var result = DProcessing.FilterMissingFiles(TestData.DestinationFilteredList, TestData.SourceCompressedOnlyFilesList_Result);
            var expectedResult = TestData.DestinationFilteredMissingList;

            Assert.IsNotNull(result);
            result = result.OrderBy(x => x);
            Utilities.AssertAreEqual(expectedResult, result, true);
        }

        [TestMethod]
        public void H_GetFileName_Correct_Value()
        {
            var result = DProcessing.GetFileName(TestData.FullPathFolderInfo.CompressedSource + "\\TestFile.txt");
            var expectedResult = "TestFile.txt";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void H_GetFileName_No_Change_Correct_Value()
        {
            var result = DProcessing.GetFileName("TestFile.txt");
            var expectedResult = "TestFile.txt";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void H_GetFileName_No_File_Empty()
        {
            var result = DProcessing.GetFileName(TestData.FullPathFolderInfo.CompressedSource + "\\");
            var expectedResult = "";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void I_GetFolderName_Correct_Value()
        {
            var result = DProcessing.GetFolderName("TestFile.txt");
            var expectedResult = "TestFile";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void I_GetFolderName_No_Change_Correct_Value()
        {
            var result = DProcessing.GetFolderName("TestFile");
            var expectedResult = "TestFile";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void I_GetFolderName_No_File_Empty()
        {
            var result = DProcessing.GetFolderName("");
            var expectedResult = "";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task J_FilterDestination_Correct_Value()
        {
            var result = await DProcessing.FilterDestination(TestData.DestinationFullList, TestData.FilterFolder);
            var expectedResult = TestData.DestinationFilteredList;

            Assert.IsNotNull(result);
            Utilities.AssertAreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task J_FilterDestination_No_Results_Correct_Value()
        {
            var result = await DProcessing.FilterDestination(TestData.FullPathList_Result, TestData.FilterFolderNoResults);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
