using Skyrim_Mod_Verification;

namespace Skyrim_Mod_UnitTest
{
	[TestClass]
	public class CompressionTests
	{
		[TestMethod]
		public void A_GetCompressedFileContent_Not_Null()
		{
			var result = DataProcessing.GetCompressedFileContent(TestData.ValidCompressedFile);
			Assert.IsNotNull(result);
		}

	}
}