using CompressedArchiveComparison.CompressedReadonlyReaders;

namespace CompressedArchiveComparison.Compressions
{
	public class ZipCompression() : AbstractCompressionBase(new ZipReader()), ICompression
	{
	}
}
