namespace CompressedArchiveComparison.Workflow
{
	public enum StateFlow { PreInit, StartWorkflow, LoadInitialData, IdentifyMissing, ExportMissing, Finished }
	public enum DisplayType { Source, Missing }

	public interface IComparisonWorkflow
	{
		/// <summary>
		/// Initialize the <see cref="IWorkflowActions"/> and load the config data
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public Task StartWorkflow(string[] args);

		/// <summary>
		/// Runs the processes to Load the Source and Destination file lists into <see cref="IWorkflowActions"/> variables for future use
		/// </summary>
		/// <returns></returns>
		public Task LoadInitialData();

		/// <summary>
		/// Validates if it's possible to output the list of source files, and writes to the console if possible
		/// </summary>
		/// <returns></returns>
		public Task DisplaySource();

		/// <summary>
		/// Executes the method to compare and store which source files have missing components from the destination
		/// </summary>
		public void IdentifyMissing();

		/// <summary>
		/// Validates if it's possible to output the list of missing files, and writes to the console if possible
		/// </summary>
		/// <returns></returns>
		public Task DisplayMissing();

		/// <summary>
		/// Execute the method to write the list of missing files to the previously specified text file
		/// </summary>
		/// <returns></returns>
		public Task ExportMissing();

		/// <summary>
		/// Checks if the specified <paramref name="type"/> is available to be displayed and has not been displayed before
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool CanDisplay(DisplayType displayType);

		/// <summary>
		/// Returns true if the internal CurrentState is <see cref="StateFlow.Finished"/> or greater
		/// </summary>
		/// <returns></returns>
		public bool IsFinished();

		/// <summary>
		/// Increments the internal CurrentState to the next <see cref="StateFlow"/> state
		/// </summary>
		public void UpdateState();

		/// <summary>
		/// Call the next method in the workflow based on the internal CurrentState then update the state when the process finishes
		/// </summary>
		/// <returns></returns>
		public Task Next();

		/// <summary>
		/// Returns true if the passed in <paramref name="state"/> is the same as the current internal internal CurrentState for Testing Only
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public bool IsState(StateFlow state);
	}
}
