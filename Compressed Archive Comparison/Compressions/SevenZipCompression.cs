using CompressedArchiveComparison.CompressedReadonlyReaders;
using CompressedArchiveComparison.Exceptions;

namespace CompressedArchiveComparison.Compressions
{
    public class SevenZipCompression(IExceptionList el) : AbstractCompressionBase(new SevenZipReader(el)), ICompression
    {
    }
}
