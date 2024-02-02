namespace CompressedArchiveComparison.Components
{
	public interface IWorkflow
	{

		/// <summary>
		/// Print to the console the list of missing files
		/// </summary>
		/// <returns></returns>
		public Task DisplayMissingFiles();

		/// <summary>
		/// Print to the console the list of compressed files
		/// </summary>
		/// <returns></returns>
		public Task DisplaySource();

		/// <summary>
		/// Export the list of missing files to the specified text file
		/// </summary>
		/// <returns></returns>
		public Task ExportMissingFiles();

		/// <summary>
		/// Compare and store the names of the files in the compressed list that do not exist in the destination list
		/// </summary>
		public void IdentifyMissingFiles();


		/// <summary>
		/// Read and store the names of all the compressed files in the source folder
		/// </summary>
		public void LoadCompressedSource();

		/// <summary>
		/// Read and Deserialize the specified Json file into the IInfo Config property
		/// </summary>
		/// <returns></returns>
		public Task LoadConfig();

		/// <summary>
		/// Read and store the names of all the files in the destination folder
		/// </summary>
		public void LoadDestination();

		/// <summary>
		/// Set the Workflow IInfo Config property, accepts optional command line <paramref name="args"/> to
		/// specify a custom json config file name to load, otherwise it uses the <paramref name="defaultConfig"/>
		/// </summary>
		/// <param name="args"></param>
		/// <param name="defaultConfig"></param>
		public void SetConfig(string[] args, string defaultConfig);
	}
}