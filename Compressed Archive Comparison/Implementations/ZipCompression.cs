using System.IO.Compression;

namespace CompressedArchiveComparison
{
	public class ZipCompression(string fileName) : AbstractCompressionBase(fileName), ICompression
	{
		public override async Task<IEnumerable<string>> GetFiles(string filePath)
		{
			try
			{
				return await Task.Run(() =>
				{
					using var compressedData = ZipFile.OpenRead(filePath);
					var fileList = new List<string>();
					foreach (var file in compressedData.Entries)
					{
						fileList.Add(file.FullName);
					}
					return fileList;
				});
			}
			catch
			{
				Console.WriteLine($"*** Invalid Compressed Archive [{filePath}], unable to retrieve contents ");
				return new List<string>();
			}
		}
	}
}
