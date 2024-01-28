using SkyrimModVerification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyrimModUnitTest
{
	[TestClass]
	public class OneOffTests
	{
		[TestMethod]
		public void E_GetCompressedFileContent_Specific()
		{
			var file = "C:\\Games\\Skyrm Downloads\\SkyrimSE\\Skyrim Script Extender (SKSE64)-30379-2-2-6-1705522967.7z";
			var result = DataProcessing.GetCompressedFileContent(file);
			var resultCount = result.Count();


			Assert.IsNotNull(result);
			Assert.AreEqual(551, resultCount);
		}
	}
}
