using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class InitializationTests
	{
		[TestMethod]
		public void A_ReadPathInfo_Not_Null()
		{
			var result = DataProcessing.ReadPathInfo();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void A_ReadPathInfo_Correct_Value()
		{
			var result = DataProcessing.ReadPathInfo();

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ConfigLocationsJson, result);
		}

		[TestMethod]
		[DataRow("TestInfo.json")]
		public void A_ReadPathInfo_Correct_Value_Param(string testData)
		{
			var result = DataProcessing.ReadPathInfo(testData);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ConfigLocationsJson, result);
		}

		[TestMethod]
		[DataRow("")]
		[DataRow("notValidJsonFile")]
		[DataRow("fileNotFound.json")]
		public void A_ReadPathInfo_Bad_Data_Handled(string testData)
		{
			var result = DataProcessing.ReadPathInfo(testData);

			Assert.IsNotNull(result);
			Assert.AreEqual("", result);
		}

		[TestMethod]
		public void B_GetAsInfo_Not_Null()
		{
			var result = DataProcessing.GetAsInfo(TestData.ConfigLocationsJson);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void B_GetAsInfo_Correct_Value()
		{
			var result = DataProcessing.GetAsInfo(TestData.ConfigLocationsJson);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ValidFolderInfo.CompressedSource, result.CompressedSource);
			Assert.AreEqual(TestData.ValidFolderInfo.DeployDestination, result.DeployDestination);
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
			Assert.AreEqual("MissingFilesFound.txt", result.ExportFileName);
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
	}
}