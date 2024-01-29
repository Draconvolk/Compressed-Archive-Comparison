using SharpCompress.Archives;

namespace CompressedArchiveComparison
{
	public class SevenZipCompression : ICompression
	{
		public string FileName { get; set; } = "";

		public SevenZipCompression() { }

		public SevenZipCompression(string fileName)
		{
			FileName = fileName;
		}

		public IEnumerable<string> GetFiles()
		{
			if (!string.IsNullOrWhiteSpace(FileName))
			{
				return GetFiles(FileName);
			}
			else
			{
				return new List<string>();
			}
		}

		public IEnumerable<string> GetFiles(string filePath)
		{
			try
			{
				using var compressedData = ArchiveFactory.Open(filePath);
				var fileList = new List<string>();
				foreach (var file in compressedData.Entries)
				{
					fileList.Add(file.Key);
				}
				return fileList;
			}
			catch
			{
				Console.WriteLine($"*** Invalid Compressed Archive [{filePath}], unable to retrieve contents ");
				return new List<string>();
			}
		}

		public string GetTypeName() => GetType().Name;
	}
}
