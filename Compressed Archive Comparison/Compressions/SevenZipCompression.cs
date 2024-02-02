using CompressedArchiveComparison.CompressedReadonlyReaders;

namespace CompressedArchiveComparison.Compressions
{
	public class SevenZipCompression(string fileName) : AbstractCompressionBase(new SevenZipReader(), fileName), ICompression
	{
	}
}
