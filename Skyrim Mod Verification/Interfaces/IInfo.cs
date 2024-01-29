namespace CompressedArchiveComparison
{
	public interface IInfo
	{
		string CompressedSource { get; set; }
		string DeployDestination { get; set; }
		/// <summary>
		/// Valid File extensions are of type txt or log
		/// </summary>
		string ExportFileName { get; set; }
		bool Verbose { get; set; }
	}
}
