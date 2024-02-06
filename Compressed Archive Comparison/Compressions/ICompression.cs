namespace CompressedArchiveComparison.Compressions
{
	public interface ICompression
	{
		public string FileName { get; set; }

		/// <summary>
		/// Uses the internal FileName value if preveiously set, otherwise will return an empty collection
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetFiles();

		/// <summary>
		/// Pass a custom FileName value to use instead of the default
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetFiles(string filePath);

		/// <summary>
		/// String name value of this type
		/// </summary>
		/// <returns></returns>
		public string GetTypeName();

		/// <summary>
		/// Set the internal filename
		/// </summary>
		/// <param name="name"></param>
		public void SetFileName(string name);
	}
}
