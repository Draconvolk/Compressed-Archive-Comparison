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

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.TestInfoJson, result);
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

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ValidFolderInfo.CompressedSource, result.CompressedSource);
			Assert.AreEqual(TestData.ValidFolderInfo.DeployDestination, result.DeployDestination);
			Assert.AreEqual(TestData.ValidFolderInfo.ExclusionsFileName, result.ExclusionsFileName);
			Assert.AreEqual(TestData.ValidFolderInfo.ExportFileName, result.ExportFileName);
			Assert.AreEqual(TestData.ValidFolderInfo.Verbose, result.Verbose);
		}

		[TestMethod]
		[DataRow("")]
		[DataRow(null)]
		[DataRow("BadData")]
		public void B_GetAsInfo_Bad_Data_Handled(string testData)
		{
			var result = DataProcessing.GetAsInfo(testData);

			Assert.IsNotNull(result);
			Assert.AreEqual("", result.CompressedSource);
			Assert.AreEqual("", result.DeployDestination);
			Assert.AreEqual("Exclusions.txt", result.ExclusionsFileName);
			Assert.AreEqual("MissingFilesFound.txt", result.ExportFileName);
			Assert.AreEqual(false, result.Verbose);
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
		public async Task D_GetCompressedFilesList_Not_Null()
		{
			var compressedlist = await DataProcessing.GetCompressedFileList(TestData.ValidFolderInfo);

			Assert.IsNotNull(compressedlist);
		}

		[TestMethod]
		public async Task D_GetCompressedFilesList_Has_Records()
		{
			var compressedlist = await DataProcessing.GetCompressedFileList(TestData.ValidFolderInfo);

			Assert.IsTrue(compressedlist.Any());
		}

		[TestMethod]
		public async Task D_GetCompressedFilesList_Empty_Data_Handled()
		{
			var compressedlist = await DataProcessing.GetCompressedFileList(TestData.EmptyFolderInfo);

			Assert.IsNotNull(compressedlist);
			Assert.IsFalse(compressedlist.Any());
		}

		[TestMethod]
		public async Task D_GetCompressedFilesList_Bad_Data_Handled()
		{
			var compressedlist = await DataProcessing.GetCompressedFileList(TestData.BadFolderInfo);

			Assert.IsNotNull(compressedlist);
			Assert.IsFalse(compressedlist.Any());
		}

		[TestMethod]
		public async Task E_GetExclusionFileText_Not_Null()
		{
			var result = await DataProcessing.GetExclusionFileText(TestData.TestDirInfo);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task E_GetExclusionFileText_Valid_Result()
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
		public async Task F_ReadFileData_Valid()
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
		public async Task G_GetExclusionFileText_Valid_Result()
		{
			var text = await DataProcessing.GetExclusionFileText(TestData.TestDirInfo);
			var result = DataProcessing.ParseExclusionFileText(text);
			var expectedResult = TestData.ExclusionFileList;

			Assert.IsNotNull(result);
			Utilities.AssertAreEqual(expectedResult, result);
		}
	}
}