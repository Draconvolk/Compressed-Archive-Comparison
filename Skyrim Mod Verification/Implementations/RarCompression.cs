using Aspose.Zip.Rar;
using System.IO.Compression;

namespace SkyrimModVerification
{
	public class RarCompression : ICompression
	{
		public string FileName { get; set; } = "";

		public RarCompression() { }

		public RarCompression(string fileName)
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

		public IEnumerable<string> GetFiles(string filePath = "")
		{
			try
			{
				using var compressedData = new RarArchive(filePath);
				var fileList = new List<string>();
				foreach (var file in compressedData.Entries)
				{
					fileList.Add(file.Name);
				}
				return fileList;
			}
			catch
			{
				return new List<string>();
			}

		}

		public string GetTypeName() => GetType().Name;
	}
}
