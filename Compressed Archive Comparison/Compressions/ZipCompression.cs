using CompressedArchiveComparison.CompressedReadonlyReaders;

namespace CompressedArchiveComparison.Compressions
{
	public class ZipCompression(string fileName) : AbstractCompressionBase(new ZipReader(), fileName), ICompression
	{
	}
}
