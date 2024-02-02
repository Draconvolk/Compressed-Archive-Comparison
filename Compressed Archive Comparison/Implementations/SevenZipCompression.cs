using CompressedArchiveComparison.Interfaces;

namespace CompressedArchiveComparison
{
    public class SevenZipCompression(string fileName) : AbstractCompressionBase(new SevenZipReader(), fileName), ICompression
	{		
	}
}
