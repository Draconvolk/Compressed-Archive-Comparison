using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class ExportTests
	{
		[TestMethod]
		public async Task A_WriteToFile_Valid()
		{
			var result = await DataProcessing.WriteToFile(TestData.FullPathMissingList, TestData.NormalizedEmptyFileName);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public async Task A_WriteToFile_Data_Valid()
		{
			var result = await DataProcessing.WriteToFile(TestData.FullPathMissingList, TestData.NormalizedEmptyFileName);
			var expectedResult = "The Following files were missing from the destination:"
								 + Environment.NewLine
								 + TestData.FullPathMissingList.FlattenToString(Environment.NewLine)
								 + Environment.NewLine;
			var writtenResult = await File.ReadAllTextAsync(TestData.NormalizedEmptyFileName);

			Assert.IsTrue(result);
			Assert.AreEqual(expectedResult, writtenResult);
		}
	}
}
