using CompressedArchiveComparison.CompressedReadonlyReaders;

namespace CompressedArchiveComparison.Compressions
{
	public class RarCompression() : AbstractCompressionBase(new RarReader()), ICompression
	{
	}
}
