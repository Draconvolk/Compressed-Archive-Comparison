﻿using CompressedArchiveComparison.CompressedReadonlyReaders;

namespace CompressedArchiveComparison.Compressions
{
    public abstract class AbstractCompressionBase(ICompressedReader reader, string fileName = "") : ICompression
    {
        public virtual string FileName { get; set; } = fileName;
        public ICompressedReader Reader { get; set; } = reader;

        public virtual IEnumerable<string> GetFiles() => !string.IsNullOrWhiteSpace(FileName) ? GetFiles(FileName) : [];

        public virtual IEnumerable<string> GetFiles(string filePath) => Reader.Read(filePath);

        public virtual string GetTypeName() => GetType().Name;
    }
}