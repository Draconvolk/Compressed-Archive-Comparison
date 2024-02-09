using CompressedArchiveComparison.Exceptions;
using System.IO.Compression;

namespace CompressedArchiveComparison.CompressedReadonlyReaders
{
    public class ZipReader(IExceptionList exceptionList) : ICompressedReader
    {
        public IEnumerable<string> Read(string filePath)
        {
            try
            {
                var records = new List<string>();
                using var compressedData = ZipFile.OpenRead(filePath);
                foreach (var file in compressedData.Entries)
                {
                    records.Add(FixForDesktop(file.FullName));
                }
                return records;
            }
            catch (Exception ex)
            {
                exceptionList.Add(ex, $"Something went wrong while reading the content of the compressed file {filePath}", $"ZipReader\\Read", [filePath]);
                return [];
            }
        }

        public string FixForDesktop(string fullPath) => fullPath.Replace('/', '\\');
    }
}
