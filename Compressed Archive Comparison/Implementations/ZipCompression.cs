using CompressedArchiveComparison.Interfaces;

namespace CompressedArchiveComparison
{
	public class ZipCompression(string fileName) : AbstractCompressionBase(new ZipReader(), fileName), ICompression
	{
	}
}
