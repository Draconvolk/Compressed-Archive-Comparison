using CompressedArchiveComparison.CompressedReadonlyReaders;

namespace CompressedArchiveComparison.Compressions
{
	public class RarCompression(string fileName) : AbstractCompressionBase(new RarReader(), fileName), ICompression
	{
	}
}
