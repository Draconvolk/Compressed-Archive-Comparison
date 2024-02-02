using CompressedArchiveComparison.Interfaces;
using System.IO.Compression;

namespace CompressedArchiveComparison
{
	public class ZipReader : ICompressedReader
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
			catch
			{
				Console.WriteLine($"*** Something went wrong while reading the content of the compressed file [{filePath}]");
				return [];
			}
		}

		public string FixForDesktop(string fullPath) => fullPath.Replace('/', '\\');
	}
}
