using CompressedArchiveComparison.Components;
using CompressedArchiveComparison.Config;

namespace CompressedArchiveComparisonTests
{
    [TestClass]
	public class InitializationTests
	{
        private static DataProcessing DProcessing => Utilities.GetInjectedObject<DataProcessing>() ?? throw new Exception("Failed to load DataProcessing");
        
		[TestMethod]
		[DataRow("TestInfo.json")]
		public async Task A_ReadPathInfo_Correct_Value_Param(string testData)
		{
			var result = await DProcessing.ReadPathInfo(testData);
			var expectedResult = TestData.TestInfoJson;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		[DataRow("")]
		[DataRow("notValidJsonFile")]
		[DataRow("fileNotFound.json")]
		public async Task A_ReadPathInfo_Bad_Data_Handled(string testData)
		{
			var result = await DProcessing.ReadPathInfo(testData);

			Assert.IsNotNull(result);
			Assert.AreEqual("", result);
		}

		[TestMethod]
		public void B_GetAsInfo_Not_Null()
		{
			var result = DProcessing.GetAsInfo(TestData.TestInfoJson);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void B_GetAsInfo_Correct_Value()
		{
			var result = DProcessing.GetAsInfo(TestData.TestInfoJson);
			var expectedResult = TestData.ValidFolderInfo;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.CompressedSource, result.CompressedSource);
			Assert.AreEqual(expectedResult.DeployDestination, result.DeployDestination);
			Assert.AreEqual(expectedResult.ExclusionsFileName, result.ExclusionsFileName);
			Assert.AreEqual(expectedResult.ExportFileName, result.ExportFileName);
			Assert.AreEqual(expectedResult.Verbose, result.Verbose);
		}

		[TestMethod]
		[DataRow("")]
		[DataRow(null)]
		[DataRow("BadData")]
		public void B_GetAsInfo_Bad_Data_Handled(string testData)
		{
			var result = DProcessing.GetAsInfo(testData);
			var expectedResult = TestData.DefaultInfo_Result;

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.CompressedSource, result.CompressedSource);
			Assert.AreEqual(expectedResult.DeployDestination, result.DeployDestination);
			Assert.AreEqual(expectedResult.ExclusionsFileName, result.ExclusionsFileName);
			Assert.AreEqual(expectedResult.ExportFileName, result.ExportFileName);
			Assert.AreEqual(expectedResult.Verbose, result.Verbose);
		}

		[TestMethod]
		public void C_IsValidInfo_IsTrue()
		{
			var result = DProcessing.IsValidInfo(TestData.ValidFolderInfo);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void C_IsValidInfo_Empty_Handled()
		{
			var result = DProcessing.IsValidInfo(TestData.EmptyFolderInfo);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void D_GetCompressedFilesList_Correct_Value()
		{
			var result = DProcessing.GetCompressedFileList(TestData.ValidFolderInfo);
			var expectedResult = TestData.CompressedSourceUnFiltered_Result;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public void D_GetCompressedFilesList_Empty_Data_Handled()
		{
			var result = DProcessing.GetCompressedFileList(TestData.EmptyFolderInfo);

			Assert.IsNotNull(result);
			Assert.IsFalse(result.Any());
		}

		[TestMethod]
		public void D_GetCompressedFilesList_Bad_Data_Handled()
		{
			var result = DProcessing.GetCompressedFileList(TestData.BadFolderInfo);

			Assert.IsNotNull(result);
			Assert.IsFalse(result.Any());
		}

		[TestMethod]
		public void D_GetCompressedOfType_Correct_Value()
		{
			var result = DProcessing.GetCompressedOfType(TestData.ValidFolderInfo, "*.rar");
			var expectedResult = "SourceDir\\TestRar.rar";
			var resultFlattened = result.FlattenToString("");

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, resultFlattened);
		}

		[TestMethod]
		public async Task E_GetExclusionFileText_Correct_Value()
		{
			var result = await DProcessing.GetExclusionFileText(TestData.TestDirInfo);
			var expectedResult = TestData.ExclusionFileList.FlattenToString(Environment.NewLine);

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		[DataRow("")]
		[DataRow("TestEmptyExclusions.txt")]
		[DataRow("BadFileName.bat")]
		public async Task E_GetExclusionFileText_Empty_File(string testData)
		{
			var testInfo = new ConfigurationInfo() { ExclusionsFileName = testData };
			var result = await DProcessing.GetExclusionFileText(testInfo);
			var expectedResult = "";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public async Task F_ReadFileData_Correct_Value()
		{
			var result = await DProcessing.ReadFileData(TestData.TestDirInfo.ExclusionsFileName);
			var expectedResult = TestData.ExclusionFileList.FlattenToString(Environment.NewLine);

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("BadFileName.bat")]
		public async Task F_ReadFileData_Invalid(string testData)
		{
			var result = await DProcessing.ReadFileData(testData);
			var expectedResult = "";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void G_ParseExclusionFileText_Correct_Value()
		{
			var flattenedData = TestData.ExclusionFileList.FlattenToString(Environment.NewLine);
			var result = DProcessing.ParseExclusionFileText(flattenedData, Environment.NewLine);
			var expectedResult = TestData.ExclusionFileList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		public void G_ParseExclusionFileText_Bad_Data(string testData)
		{
			var result = DProcessing.ParseExclusionFileText(testData, Environment.NewLine);

			Assert.IsNotNull(result);
			Assert.IsFalse(result.Any());
		}

		[TestMethod]
		[DataRow("\r\n")]
		[DataRow(", ")]
		[DataRow("|")]
		[DataRow(";")]
		public void G_ParseExclusionFileText_Separator_Correct_Value(string testData)
		{
			var testText = TestData.ExclusionFileList.FlattenToString(testData);
			var result = DProcessing.ParseExclusionFileText(testText, testData);
			var expectedResult = TestData.ExclusionFileList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public void G_ParseExclusionFileText_Separator_blank()
		{
			var testText = TestData.ExclusionFileList.FlattenToString(Environment.NewLine);
			var result = DProcessing.ParseExclusionFileText(testText, "");
			var expectedResult = TestData.ExclusionFileList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public void H_FilterSourceList_Correct_Value()
		{
			var result = DProcessing.FilterSourceList(TestData.CompressedSourceUnFiltered_Result, TestData.ExclusionFileList);
			var expectedResult = TestData.FilteredSourceList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}
	}
}