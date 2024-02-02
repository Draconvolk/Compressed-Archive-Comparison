using CompressedArchiveComparison.Components;
using CompressedArchiveComparison.Compressions;

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
		public void B_GetCompressedFileContent_Invalid_NotNull()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.InvalidCompressedZip);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_Valid_SevenZip()
		{
			var sevenZipResult = new SevenZipCompression(fileName: TestData.ValidCompressedFile7z);
			var result = sevenZipResult.GetFiles();

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.OrderBy(x => x).FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_Valid_SevenZipParam()
		{
			var sevenZipResult = new SevenZipCompression(fileName: TestData.ValidCompressedFile7z);
			var result = sevenZipResult.GetFiles(TestData.ValidCompressedFile7z);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.OrderBy(x => x).FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_InValid_SevenZip()
		{
			var sevenZipResult = new SevenZipCompression(fileName: TestData.InvalidCompressedZip);
			var result = sevenZipResult.GetFiles();

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_Valid_Rar()
		{
			var rarResult = new RarCompression(TestData.ValidCompressedFileRar);
			var result = rarResult.GetFiles();

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.OrderBy(x => x).FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_Valid_RarParam()
		{
			var rarResult = new RarCompression(TestData.ValidCompressedFileRar);
			var result = rarResult.GetFiles(TestData.ValidCompressedFileRar);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.OrderBy(x => x).FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_InValid_Rar()
		{
			var rarResult = new RarCompression(TestData.InvalidCompressedZip);
			var result = rarResult.GetFiles();

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_Valid_Zip()
		{
			var zipResult = new ZipCompression(TestData.ValidCompressedFileZip);
			var result = zipResult.GetFiles();

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.OrderBy(x => x).FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_Valid_ZipParam()
		{
			var zipResult = new ZipCompression(TestData.ValidCompressedFileZip);
			var result = zipResult.GetFiles(TestData.ValidCompressedFileZip);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.OrderBy(x => x).FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames, true);
		}

		[TestMethod]
		public void C_GetCompressedFileContent_InValid_Zip()
		{
			var zipResult = new ZipCompression(TestData.InvalidCompressedZip);
			var result = zipResult.GetFiles();

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void D_GetCompressedFileContent_Valid_SevenZip()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.ValidCompressedFile7z);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public void D_GetCompressedFileContent_InValid_SevenZip()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.InvalidCompressed7z);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(0, resultCount);
		}


		[TestMethod]
		public void D_GetCompressedFileContent_Valid_Rar()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.ValidCompressedFileRar);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public void D_GetCompressedFileContent_InValid_Rar()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.InvalidCompressedRar);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void D_GetCompressedFileContent_Valid_Zip()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.ValidCompressedFileZip);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(2, resultCount);
			var resultNames = result.FlattenToString();
			var expectedNames = "TestFile1.txt, TestFile2.txt";

			Assert.AreEqual(expectedNames, resultNames);
		}

		[TestMethod]
		public void D_GetCompressedFileContent_InValid_Zip()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.InvalidCompressedZip);

			Assert.IsNotNull(result);
			var resultCount = result.Count();

			Assert.AreEqual(0, resultCount);
		}
	}
}