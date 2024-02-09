using CompressedArchiveComparison.Components;
using CompressedArchiveComparison.Exceptions;
using CompressedArchiveComparison.Workflow;

namespace CompressedArchiveComparisonTests
{
    [TestClass]
    public class ComparisonWorkflowTests
    {
        private static DataProcessing DProcessing => Utilities.GetInjectedObject<DataProcessing>() ?? throw new Exception("Failed to load DataProcessing");
        private static IExceptionList ExList => Utilities.GetInjectedObject<IExceptionList>() ?? throw new Exception("Failed to load IExceptionList");

        [TestMethod]
        public void A_ComparisonWorkflow_Initialized_Correct_Value()
        {
            var workflowActions = new WorkflowActions(ExList, DProcessing, TestData.TestDirInfo);
            workflowActions.SetConfig(["TestInfo.json"], "TestInfo.json");
            var testWorkflow = new ComparisonWorkflow(ExList, workflowActions);

            var resultState = testWorkflow.IsState(StateFlow.PreInit);

            Assert.IsNotNull(testWorkflow);
            Assert.IsTrue(resultState);
            Assert.IsFalse(testWorkflow.CanDisplay(DisplayType.Source));
            Assert.IsFalse(testWorkflow.CanDisplay(DisplayType.Missing));
            Assert.IsFalse(testWorkflow.IsFinished());
        }

        [TestMethod]
        public async Task B_StartWorkflow_Correct_Value()
        {
            var workflowActions = new WorkflowActions(ExList, DProcessing, TestData.TestDirInfo);
            workflowActions.SetConfig(["TestInfo.json"], "TestInfo.json");
            var testWorkflow = new ComparisonWorkflow(ExList, workflowActions);
            await testWorkflow.StartWorkflow(["TestInfo.json"]);

            var resultState = testWorkflow.IsState(StateFlow.StartWorkflow);

            Assert.IsNotNull(testWorkflow);
            Assert.IsTrue(resultState);
            Assert.IsFalse(testWorkflow.CanDisplay(DisplayType.Source));
            Assert.IsFalse(testWorkflow.CanDisplay(DisplayType.Missing));
            Assert.IsFalse(testWorkflow.IsFinished());
        }

        [TestMethod]
        public async Task C_Next_LoadInitData_Correct_Value()
        {
            var workflowActions = new WorkflowActions(ExList, DProcessing, TestData.TestDirInfo);
            workflowActions.SetConfig(["TestInfo.json"], "TestInfo.json");
            var testWorkflow = new ComparisonWorkflow(ExList, workflowActions);
            await testWorkflow.StartWorkflow(["TestInfo.json"]);
            await testWorkflow.Next();

            var resultState = testWorkflow.IsState(StateFlow.LoadInitialData);

            Assert.IsNotNull(testWorkflow);
            Assert.IsTrue(resultState);
            Assert.IsTrue(testWorkflow.CanDisplay(DisplayType.Source));
            Assert.IsFalse(testWorkflow.CanDisplay(DisplayType.Missing));
            Assert.IsFalse(testWorkflow.IsFinished());
        }

        [TestMethod]
        public async Task D_Next_IdentifyMissing_Correct_Value()
        {
            var workflowActions = new WorkflowActions(ExList, DProcessing, TestData.TestDirInfo);
            workflowActions.SetConfig(["TestInfo.json"], "TestInfo.json");
            var testWorkflow = new ComparisonWorkflow(ExList, workflowActions);
            await testWorkflow.StartWorkflow(["TestInfo.json"]);
            await testWorkflow.Next();
            await testWorkflow.Next();

            var resultState = testWorkflow.IsState(StateFlow.IdentifyMissing);

            Assert.IsNotNull(testWorkflow);
            Assert.IsTrue(resultState);
            Assert.IsTrue(testWorkflow.CanDisplay(DisplayType.Source));
            Assert.IsTrue(testWorkflow.CanDisplay(DisplayType.Missing));
            Assert.IsFalse(testWorkflow.IsFinished());
        }

        [TestMethod]
        public async Task E_Next_ExportMissing_Correct_Value()
        {
            var workflowActions = new WorkflowActions(ExList, DProcessing, TestData.TestDirInfo);
            workflowActions.SetConfig(["TestInfo.json"], "TestInfo.json");
            var testWorkflow = new ComparisonWorkflow(ExList, workflowActions);
            await testWorkflow.StartWorkflow(["TestInfo.json"]);
            await testWorkflow.Next();
            await testWorkflow.Next();
            await testWorkflow.Next();

            var resultState = testWorkflow.IsState(StateFlow.ExportMissing);

            Assert.IsNotNull(testWorkflow);
            Assert.IsTrue(resultState);
            Assert.IsTrue(testWorkflow.CanDisplay(DisplayType.Source));
            Assert.IsTrue(testWorkflow.CanDisplay(DisplayType.Missing));
            Assert.IsFalse(testWorkflow.IsFinished());
        }

        [TestMethod]
        public async Task F_Next_Finished_Correct_Value()
        {
            var workflowActions = new WorkflowActions(ExList, DProcessing, TestData.TestDirInfo);
            workflowActions.SetConfig(["TestInfo.json"], "TestInfo.json");
            var testWorkflow = new ComparisonWorkflow(ExList, workflowActions);
            await testWorkflow.StartWorkflow(["TestInfo.json"]);
            await testWorkflow.Next();
            await testWorkflow.Next();
            await testWorkflow.Next();
            await testWorkflow.Next();

            var resultState = testWorkflow.IsState(StateFlow.Finished);

            Assert.IsNotNull(testWorkflow);
            Assert.IsTrue(resultState);
            Assert.IsTrue(testWorkflow.CanDisplay(DisplayType.Source));
            Assert.IsTrue(testWorkflow.CanDisplay(DisplayType.Missing));
            Assert.IsTrue(testWorkflow.IsFinished());
        }
    }
}
