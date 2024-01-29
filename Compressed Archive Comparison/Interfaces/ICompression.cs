namespace CompressedArchiveComparison
{
	public interface ICompression
	{
		public string FileName { get; set; }

		/// <summary>
		/// Uses the internal FileName value if preveiously set, otherwise will return null
		/// </summary>
		/// <returns></returns>
		public Task<IEnumerable<string>> GetFiles();
		public Task<IEnumerable<string>> GetFiles(string filePath);

		/// <summary>
		/// String name value of this type
		/// </summary>
		/// <returns></returns>
		public string GetTypeName();
	}
}
