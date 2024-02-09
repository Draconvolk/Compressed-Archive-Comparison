using CompressedArchiveComparison.Factories;

namespace CompressedArchiveComparisonTests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        [DataRow(",")]
        [DataRow("|")]
        [DataRow(" = ")]
        public void A_FlattenToString_Valid(string testData)
        {
            var testList = TestData.ExclusionFileList;
            var result = Utilities.FlattenToString(testList, testData);
            var expectedResult = (testList[0] + testData + testList[1]).Trim();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [DataRow(".rar", "RarCompression")]
        [DataRow(".7z", "SevenZipCompression")]
        [DataRow(".zip", "ZipCompression")]
        public void B_GetInjectedObject_ICompressionFactory_Not_Null(string testData, string expectedResult)
        {
            var result = Utilities.GetInjectedObject<ICompressionFactory>();

            Assert.IsNotNull(result);
            var compressionResult = result.GetCompressionType(testData);

            Assert.IsNotNull(compressionResult);
            Assert.AreEqual(expectedResult, compressionResult.GetTypeName());
        }
    }
}
