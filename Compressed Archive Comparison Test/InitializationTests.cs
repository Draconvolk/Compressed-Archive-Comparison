using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class InitializationTests
	{
		[TestMethod]
		[DataRow("TestInfo.json")]
		public async Task A_ReadPathInfo_Correct_Value_Param(string testData)
		{
			var result = await DataProcessing.ReadPathInfo(testData);
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
			var result = await DataProcessing.ReadPathInfo(testData);

			Assert.IsNotNull(result);
			Assert.AreEqual("", result);
		}

		[TestMethod]
		public void B_GetAsInfo_Not_Null()
		{
			var result = DataProcessing.GetAsInfo(TestData.TestInfoJson);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void B_GetAsInfo_Correct_Value()
		{
			var result = DataProcessing.GetAsInfo(TestData.TestInfoJson);
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
			var result = DataProcessing.GetAsInfo(testData);
			var expectedResult = TestData.ExpectedDefaultInfo;

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
			var result = DataProcessing.IsValidInfo(TestData.ValidFolderInfo);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void C_IsValidInfo_Empty_Handled()
		{
			var result = DataProcessing.IsValidInfo(TestData.EmptyFolderInfo);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void D_GetCompressedFilesList_Correct_Value()
		{
			var result = DataProcessing.GetCompressedFileList(TestData.ValidFolderInfo);
			var expectedResult = TestData.LoadCompressedSourceExpectedValue;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}

		[TestMethod]
		public void D_GetCompressedFilesList_Empty_Data_Handled()
		{
			var result = DataProcessing.GetCompressedFileList(TestData.EmptyFolderInfo);

			Assert.IsNotNull(result);
			Assert.IsFalse(result.Any());
		}

		[TestMethod]
		public void D_GetCompressedFilesList_Bad_Data_Handled()
		{
			var result = DataProcessing.GetCompressedFileList(TestData.BadFolderInfo);

			Assert.IsNotNull(result);
			Assert.IsFalse(result.Any());
		}

		[TestMethod]
		public void D_GetCompressedOfType_Correct_Value()
		{
			var result = DataProcessing.GetCompressedOfType(TestData.ValidFolderInfo, "*.rar");
			var expectedResult = "SourceDir\\TestRar.rar";
			var resultFlattened = result.FlattenToString("");

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, resultFlattened);
		}

		[TestMethod]
		public async Task E_GetExclusionFileText_Correct_Value()
		{
			var result = await DataProcessing.GetExclusionFileText(TestData.TestDirInfo);
			var expectedResult = TestData.ExclusionFileList.FlattenToString(Environment.NewLine);

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public async Task E_GetExclusionFileText_NoName()
		{
			var result = await DataProcessing.GetExclusionFileText(TestData.EmptyFolderInfo);
			var expectedResult = "";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public async Task F_ReadFileData_Correct_Value()
		{
			var result = await DataProcessing.ReadFileData(TestData.TestDirInfo.ExclusionsFileName);
			var expectedResult = TestData.ExclusionFileList.FlattenToString(Environment.NewLine);

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("BadFIleName.bat")]
		public async Task F_ReadFileData_Invalid(string testData)
		{
			var result = await DataProcessing.ReadFileData(testData);
			var expectedResult = "";

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public async Task G_GetExclusionFileText_Correct_Value()
		{
			var text = await DataProcessing.GetExclusionFileText(TestData.TestDirInfo);
			var result = DataProcessing.ParseExclusionFileText(text);
			var expectedResult = TestData.ExclusionFileList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}
	}
}