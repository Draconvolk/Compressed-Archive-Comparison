using Skyrim_Mod_Verification;

namespace Skyrim_Mod_UnitTest
{
	[TestClass]
	public class InitializationTests
	{
		[TestMethod]
		public void JsonLoadNotNull()
		{
			Assert.IsNotNull(DataProcessing.ReadPathInfo());
		}

		[TestMethod]
		public void JsonLoadCorrectValue()
		{
			Assert.AreEqual(DataProcessing.ReadPathInfo(), TestData.ConfigLocationsJson);
		}

		[TestMethod]
		public void JsonDeserializationNotNull()
		{
			Assert.IsNotNull(DataProcessing.GetAsInfo(TestData.ConfigLocationsJson));
		}

		[TestMethod]
		public void JsonDeserializationValue()
		{
			var testVal = DataProcessing.GetAsInfo(TestData.ConfigLocationsJson);
			Assert.IsNotNull(testVal);
			Console.WriteLine(testVal);
			Assert.AreEqual(TestData.ValidFolderInfo.CompressedSource, testVal.CompressedSource);
			Assert.AreEqual(TestData.ValidFolderInfo.DeployDestination, testVal.DeployDestination);
		}

		[TestMethod]
		[DataRow("")]
		[DataRow(null)]
		[DataRow("BadData")]
		public void JsonDeserializationValueEmpty(string testData)
		{
			var testVal = DataProcessing.GetAsInfo(testData);
			Assert.IsNotNull(testVal);
			Assert.AreEqual("", testVal.CompressedSource);
			Assert.AreEqual("", testVal.DeployDestination);
		}

		[TestMethod]
		public void FolderLocationInfoHasValuesTrue()
		{
			Assert.IsTrue(DataProcessing.IsValidInfo(TestData.ValidFolderInfo));
		}

		[TestMethod]
		public void FolderLocationInfoHasValuesFalse()
		{
			Assert.IsFalse(DataProcessing.IsValidInfo(TestData.EmptyFolderInfo));
		}

		[TestMethod]
		public void DirectoryGetFilesTrue()
		{
			var compressedlist = DataProcessing.GetCompressedFileList(TestData.ValidFolderInfo);
			Assert.IsNotNull(compressedlist);
			Assert.IsTrue(compressedlist.Any());
		}

		[TestMethod]
		public void DirectoryGetFilesFalse()
		{
			var compressedlist = DataProcessing.GetCompressedFileList(TestData.EmptyFolderInfo);
			Assert.IsNotNull(compressedlist);
			Assert.IsFalse(compressedlist.Any());
		}
	}

	public static class TestData
	{
		public static readonly string ConfigLocationsJson = "{\r\n\t\"CompressedSource\": \"C:\\\\Games\\\\Skyrm Downloads\\\\SkyrimSE\",\r\n\t\"DeployDestination\": \"C:\\\\Games\\\\Skyrim Mods\"\r\n}\r\n";
		public static readonly FolderLocationInfo ValidFolderInfo = new() { CompressedSource = @"C:\Games\Skyrm Downloads\SkyrimSE", DeployDestination = @"C:\Games\Skyrim Mods" };
		public static readonly FolderLocationInfo EmptyFolderInfo = new() { CompressedSource = "", DeployDestination = "" };
	}
}