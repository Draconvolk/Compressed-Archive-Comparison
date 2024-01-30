using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class CompressionTests
	{
		[TestMethod]
		[DataRow("TestZip.zip")]
		[DataRow("TestSevenZip.7z")]
		[DataRow("TestRar.rar")]
		public void A_GetCompressionFactory_Not_Null(string type)
		{
			var result = CompressionFactory.GetCompressionType(type);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		[DataRow("")]
		[DataRow(null)]
		[DataRow("BadData")]
		public void A_GetCompressionFactory_Bad_Data_IsNull(string type)
		{
			var result = CompressionFactory.GetCompressionType(type);

			Assert.IsNull(result);
		}

		[TestMethod]
		[DataRow("TestZip.zip")]
		[DataRow("TestSevenZip.7z")]
		[DataRow("TestRar.rar")]
		public void A_GetCompressionFactory_Correct_Type(string type)
		{
			var result = CompressionFactory.GetCompressionType(type);

			Assert.IsNotNull(result);
			switch (type[type.LastIndexOf('.')..])
			{
				case ".zip":
					Assert.AreEqual("ZipCompression", result.GetTypeName());
					break;
				case ".7z":
					Assert.AreEqual("SevenZipCompression", result.GetTypeName());
					break;
				case ".rar":
					Assert.AreEqual("RarCompression", result.GetTypeName());
					break;
				default:
					break;
			}
		}

		[TestMethod]
		public void B_GetCompressedFileContent_Not_Null()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.ValidCompressedFile);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task B_GetCompressedFileContent_Invalid_NotNull()
		{
			var result = await DataProcessing.GetCompressedFileContent(TestData.InvalidCompressedZip);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_Valid_SevenZip()
		{
			var sevenZipResult = new SevenZipCompression(TestData.ValidCompressedFile7z);
			var result = await sevenZipResult.GetFiles();
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_Valid_SevenZipParam()
		{
			var sevenZipResult = new SevenZipCompression(TestData.ValidCompressedFile7z);
			var result = await sevenZipResult.GetFiles(TestData.ValidCompressedFile7z);
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_InValid_SevenZip()
		{
			var sevenZipResult = new SevenZipCompression(TestData.InvalidCompressedZip);
			var result = await sevenZipResult.GetFiles();
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_Valid_Rar()
		{
			var rarResult = new RarCompression(TestData.ValidCompressedFileRar);
			var result = await rarResult.GetFiles();
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_Valid_RarParam()
		{
			var rarResult = new RarCompression(TestData.ValidCompressedFileRar);
			var result = await rarResult.GetFiles(TestData.ValidCompressedFileRar);
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_InValid_Rar()
		{
			var rarResult = new RarCompression(TestData.InvalidCompressedZip);
			var result = await rarResult.GetFiles();
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_Valid_Zip()
		{
			var zipResult = new ZipCompression(TestData.ValidCompressedFileZip);
			var result = await zipResult.GetFiles();
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_Valid_ZipParam()
		{
			var zipResult = new ZipCompression(TestData.ValidCompressedFileZip);
			var result = await zipResult.GetFiles(TestData.ValidCompressedFileZip);
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task C_GetCompressedFileContent_InValid_Zip()
		{
			var zipResult = new ZipCompression(TestData.InvalidCompressedZip);
			var result = await zipResult.GetFiles();
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public async Task D_GetCompressedFileContent_Valid_SevenZip()
		{
			var result = await DataProcessing.GetCompressedFileContent(TestData.ValidCompressedFile7z);
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task D_GetCompressedFileContent_InValid_SevenZip()
		{
			var result = await DataProcessing.GetCompressedFileContent(TestData.InvalidCompressed7z);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}


		[TestMethod]
		public async Task D_GetCompressedFileContent_Valid_Rar()
		{
			var result = await DataProcessing.GetCompressedFileContent(TestData.ValidCompressedFileRar);
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task D_GetCompressedFileContent_InValid_Rar()
		{
			var result = await DataProcessing.GetCompressedFileContent(TestData.InvalidCompressedRar);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public async Task D_GetCompressedFileContent_Valid_Zip()
		{
			var result = await DataProcessing.GetCompressedFileContent(TestData.ValidCompressedFileZip);
			var resultCount = result.Count();
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.IsNotNull(result);
			Assert.AreEqual(2, resultCount);
			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public async Task D_GetCompressedFileContent_InValid_Zip()
		{
			var result = await DataProcessing.GetCompressedFileContent(TestData.InvalidCompressedZip);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}
	}
}