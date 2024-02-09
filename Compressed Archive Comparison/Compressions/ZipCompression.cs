using CompressedArchiveComparison.CompressedReadonlyReaders;
using CompressedArchiveComparison.Exceptions;

namespace CompressedArchiveComparison.Compressions
{
    public class ZipCompression(IExceptionList el) : AbstractCompressionBase(new ZipReader(el)), ICompression
    {
    }
}
