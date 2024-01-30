using CompressedArchiveComparison;

namespace CompressedArchiveComparisonTests
{
	[TestClass]
	public class WorkflowTests
	{
		[TestMethod]
		[DataRow("TestInfo.Json")]
		[DataRow(null)]
		[DataRow("")]
		public void A_SetConfig_Correct_Value(string testData)
		{
			var defaultValue = "TestInfo.Json";
			var args = new string[] { testData };
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig(args, defaultValue);

			Assert.AreEqual(defaultValue, workflow.Config);
		}

		[TestMethod]
		[DataRow("Config1.Json")]
		[DataRow("Config2.Json")]
		[DataRow("Config3.Json")]
		public void A_SetConfig_Valid_Args_Value(string testData)
		{
			var defaultValue = "TestInfo.Json";
			var args = new string[] { testData };
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig(args, defaultValue);

			Assert.AreEqual(testData, workflow.Config);
		}

		[TestMethod]
		public void B_GetInfoFromJson_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var result = Workflow.GetInfoFromJson(defaultValue);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ValidFolderInfo.CompressedSource, result.CompressedSource);
			Assert.AreEqual(TestData.ValidFolderInfo.DeployDestination, result.DeployDestination);
			Assert.AreEqual(TestData.ValidFolderInfo.ExportFileName, result.ExportFileName);
			Assert.AreEqual(TestData.ValidFolderInfo.Verbose, result.Verbose);
		}

		[TestMethod]
		public void B_LoadConfig_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			workflow.LoadConfig();
			var result = workflow.ConfigInfo;

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ValidFolderInfo.CompressedSource, result.CompressedSource);
			Assert.AreEqual(TestData.ValidFolderInfo.DeployDestination, result.DeployDestination);
			Assert.AreEqual(TestData.ValidFolderInfo.ExportFileName, result.ExportFileName);
			Assert.AreEqual(TestData.ValidFolderInfo.Verbose, result.Verbose);
		}

		[TestMethod]
		public async Task C_LoadCompressedSource_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			workflow.LoadConfig();
			await workflow.LoadCompressedSource();
			var sourceList = workflow.SourceList;
			var expectedResult = TestData.LoadCompressedSourceExpectedValue;

			Utilities.AssertAreEqual(sourceList, expectedResult);
		}

		[TestMethod]
		public async Task D_LoadDestination_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			workflow.LoadConfig();
			await workflow.LoadCompressedSource();
			await workflow.LoadDestination();
			var destinationList = workflow.DestinationList;
			var expectedResult = TestData.LoadDestinationExpectedValue;

			Utilities.AssertAreEqual(destinationList, expectedResult, true);
		}

		[TestMethod]
		public async Task E_IdentifyMissingFiles_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			workflow.LoadConfig();
			await workflow.LoadCompressedSource();
			await workflow.LoadDestination();
			await workflow.IdentifyMissingFiles();
			var missingFiles = workflow.MissingFiles;
			var expectedResult = TestData.IdentifyMissingFilesExpectedValue;

			Utilities.AssertAreEqual(missingFiles, expectedResult, true);
		}
	}
}
