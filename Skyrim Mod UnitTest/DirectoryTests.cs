using SkyrimModVerification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrimModUnitTest
{
	[TestClass]
	public class DirectoryTests
	{

		[TestMethod]
		public void A_GetDirectoryFileList_Not_Null()
		{
			var testInfo = TestData.TestDirInfo;
			var result = DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(6, resultCount);
		}

		[TestMethod]
		public void A_GetDirectoryFileList_Count_Verified()
		{
			var testInfo = TestData.TestDirInfo;
			var result = DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(6, resultCount);

			//foreach(var r in result)
			//{
			//	Console.WriteLine($"Found value: {r}");
			//}
		}

		[TestMethod]
		public void A_GetDirectoryFileList_Empty_Not_Null()
		{
			var testInfo = TestData.EmptyFolderInfo;
			var result = DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void A_GetDirectoryFileList_Empty_Zero_Found()
		{
			var testInfo = TestData.EmptyFolderInfo;
			var result = DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void A_GetDirectoryFileList_Invalid_Not_Null()
		{
			var testInfo = TestData.EmptyFolderInfo;
			var result = DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
		}


		[TestMethod]
		public void A_GetDirectoryFileList_Invalid_Zero_Found()
		{
			var testInfo = TestData.EmptyFolderInfo;
			var result = DataProcessing.GetDirectoryFileList(testInfo);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(0, resultCount);
		}

		[TestMethod]
		public void B_TransformList_Valid_Result()
		{
			var result = DataProcessing.TransformList(TestData.ValidSourceList, TestData.SourceToRemove).ToList();
			var expectedResult = TestData.ExpectedResultList;
			var actualResultsFlattened = result.Aggregate((a, b) => $"{a}, {b}").Trim();
			var expectedResultsFlattened = expectedResult.Aggregate((a, b) => $"{a}, {b}").Trim();

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.Count, result.Count);
			Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
		}

		[TestMethod]
		public void B_TransformList_Valid_NoChange()
		{
			var result = DataProcessing.TransformList(TestData.ValidSourceList, "Nothing");
			var expectedResult = TestData.ValidSourceList;
			var actualResultsFlattened = result.Aggregate((a, b) => $"{a}, {b}").Trim();
			var expectedResultsFlattened = expectedResult.Aggregate((a, b) => $"{a}, {b}").Trim();

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.Count, result.ToList().Count);
			Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
		}

		[TestMethod]
		public void C_GetMissingSourceFiles_Valid_Result()
		{
			var result = DataProcessing.GetMissingSourceFiles(TestData.FullPathFolderInfo, TestData.FullPathSourcetList, TestData.FullPathResultList);
			var expectedResult = new List<string>() { $"{TestData.ValidDir}\\TestFile2.txt" };
			var actualResultsFlattened = result.Aggregate((a, b) => $"{a}, {b}").Trim();
			var expectedResultsFlattened = expectedResult.Aggregate((a, b) => $"{a}, {b}").Trim();

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult.Count, result.ToList().Count);
			Assert.AreEqual(expectedResultsFlattened, actualResultsFlattened);
		}
	}
}
