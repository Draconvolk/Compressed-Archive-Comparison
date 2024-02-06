using CompressedArchiveComparison.Components;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class OneOffTests
	{
		private readonly CompressionResolver _resolver;

		public OneOffTests() => _resolver = Utilities.GetCompressionResolver() ?? throw new Exception("Failed to load Compression Resolver");


		/// <summary>
		/// One off Test method. There is some strange encoding for this 7z file that required
		/// changing compression libraries to decompress it due to licensing
		/// </summary>
		[Ignore]
		[TestMethod]
		public void A_GetCompressedFileContent_Specific()
		{
			var file = "C:\\Games\\Skyrim Downloads\\SkyrimSE\\Skyrim Script Extender (SKSE64)-30379-2-2-6-1705522967.7z";
			var result = DataProcessing.GetCompressedFileContent(_resolver, file);
			var resultCount = result.Count();

			Assert.IsNotNull(result);
			Assert.AreEqual(551, resultCount);
		}
	}
}
