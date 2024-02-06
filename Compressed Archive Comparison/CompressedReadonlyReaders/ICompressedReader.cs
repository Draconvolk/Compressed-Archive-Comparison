namespace CompressedArchiveComparison.CompressedReadonlyReaders
{
    public interface ICompressedReader
    {
        public IEnumerable<string> Read(string filePath);
        public string FixForDesktop(string fullPath);
    }
}
