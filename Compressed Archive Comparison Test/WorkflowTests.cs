using CompressedArchiveComparison.Components;

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
		public async Task B_GetInfoFromJson_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var result = await Workflow.GetInfoFromJson(defaultValue);

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ValidFolderInfo.CompressedSource, result.CompressedSource);
			Assert.AreEqual(TestData.ValidFolderInfo.DeployDestination, result.DeployDestination);
			Assert.AreEqual(TestData.ValidFolderInfo.ExportFileName, result.ExportFileName);
			Assert.AreEqual(TestData.ValidFolderInfo.Verbose, result.Verbose);
		}

		[TestMethod]
		public async Task B_LoadConfig_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			var result = workflow.ConfigInfo;

			Assert.IsNotNull(result);
			Assert.AreEqual(TestData.ValidFolderInfo.CompressedSource, result.CompressedSource);
			Assert.AreEqual(TestData.ValidFolderInfo.DeployDestination, result.DeployDestination);
			Assert.AreEqual(TestData.ValidFolderInfo.ExportFileName, result.ExportFileName);
			Assert.AreEqual(TestData.ValidFolderInfo.Verbose, result.Verbose);
		}

		[TestMethod]
		public async Task C_LoadCompressedSource_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			await workflow.LoadCompressedSource();
			var sourceList = workflow.SourceList;
			var exclusionList = workflow.ExclusionList;
			var expectedResult = TestData.FilteredSourceList;
			var expectedResult2 = TestData.ExclusionFileList;

			Utilities.AssertAreEqual(expectedResult, sourceList);
			Utilities.AssertAreEqual(expectedResult2, exclusionList);
		}

		[TestMethod]
		public async Task C_GetFileList_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			var sourceList = Workflow.GetFileList(workflow.ConfigInfo);
			var expectedResult = TestData.CompressedSourceUnFiltered_Result;

			Utilities.AssertAreEqual(expectedResult, sourceList);
		}

		[TestMethod]
		public async Task D_LoadDestination_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			await workflow.LoadCompressedSource();
			workflow.LoadDestination();
			var destinationList = workflow.DestinationList.OrderBy(x => x);
			var expectedResult = TestData.LoadDestination_Result;

			Utilities.AssertAreEqual(expectedResult, destinationList);
		}

		[TestMethod]
		public async Task E_IdentifyMissingFiles_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			var tasks = new Task[]
			{
				Task.Factory.StartNew(workflow.LoadCompressedSource),
				Task.Factory.StartNew(workflow.LoadDestination)
			};
			Task.WaitAll(tasks);
			workflow.IdentifyMissingFiles();
			var missingFiles = workflow.MissingFiles.OrderBy(x => x);
			var expectedResult = TestData.IdentifyMissingFiles_Result;

			Utilities.AssertAreEqual(expectedResult, missingFiles);
		}

		[TestMethod]
		public async Task E_GetFileExclusions_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			var exclusionList = await Workflow.GetFileExclusions(workflow.ConfigInfo);
			var expectedResult = TestData.ExclusionFileList;

			Utilities.AssertAreEqual(expectedResult, exclusionList);
		}

		[TestMethod]
		public async Task E_FilterSourceList_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			var exclusionList = await Workflow.GetFileExclusions(workflow.ConfigInfo);
			var sourceList = Workflow.GetFileList(workflow.ConfigInfo);
			var filteredSource= Workflow.FilterSourceList(sourceList, exclusionList);
			var expectedResult = TestData.FilteredSourceList;

			Utilities.AssertAreEqual(expectedResult, filteredSource);
		}

		[TestMethod]
		public async Task F_ExportMissingFiles_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			var tasks = new Task[]
			{
				Task.Factory.StartNew(workflow.LoadCompressedSource),
				Task.Factory.StartNew(workflow.LoadDestination)
			};
			Task.WaitAll(tasks);
			workflow.IdentifyMissingFiles();
			await workflow.ExportMissingFiles();
			var writtenResult = await File.ReadAllTextAsync(TestData.NormalizedEmptyFileName);
			var expectedResult = "The Following files were missing from the destination:"
								 + Environment.NewLine
								 + TestData.IdentifyMissingFiles_Result.OrderBy(x => x).FlattenToString(Environment.NewLine)
								 + Environment.NewLine;

			Assert.AreEqual(expectedResult, writtenResult);
		}

		[TestMethod]
		public async Task F_ExportToFile_Data_Correct_Value()
		{
			var defaultValue = "TestInfo.Json";
			var workflow = new Workflow(TestData.ValidFolderInfo);
			workflow.SetConfig([], defaultValue);
			await workflow.LoadConfig();
			var tasks = new Task[]
			{
				Task.Factory.StartNew(workflow.LoadCompressedSource),
				Task.Factory.StartNew(workflow.LoadDestination)
			};
			Task.WaitAll(tasks);
			workflow.IdentifyMissingFiles();
			await Workflow.ExportToFile(workflow.MissingFiles, workflow.ConfigInfo);
			var writtenResult = await File.ReadAllTextAsync(TestData.NormalizedEmptyFileName);
			var expectedResult = "The Following files were missing from the destination:"
								 + Environment.NewLine
								 + TestData.IdentifyMissingFiles_Result.OrderBy(x => x).FlattenToString(Environment.NewLine)
								 + Environment.NewLine;

			Assert.AreEqual(expectedResult, writtenResult);
		}
	}
}
