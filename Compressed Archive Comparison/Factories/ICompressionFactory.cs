using CompressedArchiveComparison.Compressions;

namespace CompressedArchiveComparison.Factories
{
    public interface ICompressionFactory
    {
        /// <summary>
        /// Returns an appropriate compression object based on the fileName extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        ICompression? GetCompressionType(string fileName);
    }
}