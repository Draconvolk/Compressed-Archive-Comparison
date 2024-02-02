using CompressedArchiveComparison.Interfaces;

namespace CompressedArchiveComparison
{
	public abstract class AbstractCompressionBase(ICompressedReader reader, string fileName = "") : ICompression
	{
		public virtual string FileName { get; set; } = fileName;
		public ICompressedReader Reader { get; set; } = reader;
		public ParallelOptions ParaOptions { get; set; } = new() { };

		public virtual IEnumerable<string> GetFiles() => !string.IsNullOrWhiteSpace(FileName) ? GetFiles(FileName) : [];

		public virtual IEnumerable<string> GetFiles(string filePath) => Reader.Read(filePath);

		public virtual string GetTypeName() => GetType().Name;
	}
}