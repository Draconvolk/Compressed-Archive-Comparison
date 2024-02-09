using CompressedArchiveComparison.Exceptions;
using SharpCompress.Archives;

namespace CompressedArchiveComparison.CompressedReadonlyReaders
{
    public class SevenZipReader(IExceptionList exceptionList) : ICompressedReader
    {
        public IEnumerable<string> Read(string filePath)
        {
            try
            {
                var records = new List<string>();
                using var compressedData = ArchiveFactory.Open(filePath);
                foreach (var file in compressedData.Entries)
                {
                    records.Add(FixForDesktop(file.Key));
                }
                return records;
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, $"Something went wrong while reading the content of the compressed file {filePath}", $"SevenZipReader\\Read", [filePath]);
                return [];
            }
        }

        public string FixForDesktop(string fullPath) => fullPath.Replace('/', '\\');
    }
}
