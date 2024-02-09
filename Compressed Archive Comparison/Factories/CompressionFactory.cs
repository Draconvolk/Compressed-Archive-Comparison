using CompressedArchiveComparison.Components;
using CompressedArchiveComparison.Compressions;
using CompressedArchiveComparison.Exceptions;

namespace CompressedArchiveComparison.Factories
{
    public class CompressionFactory(IExceptionList exceptionList, CompressionResolver resolverAccessor) : ICompressionFactory
    {
        private static readonly string CF = "CompressionFactory";

        public const string zip = ".zip";
        public const string sevenZ = ".7z";
        public const string rar = ".rar";

        public ICompression? GetCompressionType(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || fileName.LastIndexOf('.') == -1)
            {
                exceptionList.Add(new Exception("Get compression failed. Invalid filename"), $"Can't get compression type due to an invalid filename {fileName}", $"{CF}\\GetCompressionType", [fileName]);
                return null;
            }
            try
            {
                var type = fileName[fileName.LastIndexOf('.')..];
                var compression = resolverAccessor(type);
                compression.SetFileName(fileName);
                return compression;
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, $"Something went wrong trying to get the compression type from filename {fileName}", $"{CF}\\GetCompressionType", [fileName]);
                return null;
            }
        }
    }
}
