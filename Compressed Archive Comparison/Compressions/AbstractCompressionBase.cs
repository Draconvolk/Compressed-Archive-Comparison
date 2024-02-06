using CompressedArchiveComparison.CompressedReadonlyReaders;

namespace CompressedArchiveComparison.Compressions
{
	public abstract class AbstractCompressionBase(ICompressedReader reader) : ICompression
	{
		public virtual string FileName { get; set; } = "";
		public ICompressedReader Reader { get; set; } = reader;

		public virtual IEnumerable<string> GetFiles() => !string.IsNullOrWhiteSpace(FileName) ? GetFiles(FileName) : [];

		public virtual IEnumerable<string> GetFiles(string filePath) => Reader.Read(filePath);

		public virtual string GetTypeName() => GetType().Name;

		public virtual void SetFileName(string name) => FileName = name;
	}
}