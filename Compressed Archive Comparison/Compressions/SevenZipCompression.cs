using CompressedArchiveComparison.CompressedReadonlyReaders;

namespace CompressedArchiveComparison.Compressions
{
	public class SevenZipCompression() : AbstractCompressionBase(new SevenZipReader()), ICompression
	{
	}
}
