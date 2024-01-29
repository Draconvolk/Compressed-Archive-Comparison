using System.IO.Compression;

namespace CompressedArchiveComparison
{
	public class ZipCompression : ICompression
	{
		public string FileName { get; set; } = "";

		public ZipCompression() { }

		public ZipCompression(string fileName)
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
				using var compressedData = ZipFile.OpenRead(filePath);
				var fileList = new List<string>();
				foreach (var file in compressedData.Entries)
				{
					fileList.Add(file.FullName);
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
