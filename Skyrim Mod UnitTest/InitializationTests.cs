using Skyrim_Mod_Verification;

namespace Skyrim_Mod_UnitTest
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
		public void D_GetCompressedFilesList_Not_Null()
		{
			var compressedlist = DataProcessing.GetCompressedFileList(TestData.ValidFolderInfo);
			Assert.IsNotNull(compressedlist);
		}

		[TestMethod]
		public void D_GetCompressedFilesList_Has_Records()
		{
			var compressedlist = DataProcessing.GetCompressedFileList(TestData.ValidFolderInfo);
			Assert.IsTrue(compressedlist.Any());
		}

		[TestMethod]
		public void D_GetCompressedFilesList_Empty_Data_Handled()
		{
			var compressedlist = DataProcessing.GetCompressedFileList(TestData.EmptyFolderInfo);
			Assert.IsNotNull(compressedlist);
			Assert.IsFalse(compressedlist.Any());
		}
		[TestMethod]
		public void D_GetCompressedFilesList_Bad_Data_Handled()
		{
			var compressedlist = DataProcessing.GetCompressedFileList(TestData.BadFolderInfo);
			Assert.IsNotNull(compressedlist);
			Assert.IsFalse(compressedlist.Any());
		}
	}
}