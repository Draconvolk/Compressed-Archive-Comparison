using CompressedArchiveComparison.CompressedReadonlyReaders;
using CompressedArchiveComparison.Exceptions;

namespace CompressedArchiveComparison.Compressions
{
    public class RarCompression(IExceptionList el) : AbstractCompressionBase(new RarReader(el)), ICompression
    {
    }
}
