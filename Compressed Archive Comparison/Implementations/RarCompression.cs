using CompressedArchiveComparison.Interfaces;

namespace CompressedArchiveComparison
{
	public class RarCompression(string fileName) : AbstractCompressionBase(new RarReader(), fileName), ICompression
	{		
	}
}
