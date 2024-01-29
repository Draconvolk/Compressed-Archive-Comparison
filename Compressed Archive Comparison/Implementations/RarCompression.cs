using SharpCompress.Archives;

namespace CompressedArchiveComparison
{
	public class RarCompression : AbstractCompressionBase, ICompression
	{
		public RarCompression() { }

		public RarCompression(string fileName) : base(fileName) { }

		public override async Task<IEnumerable<string>> GetFiles(string filePath)
		{
			try
			{
				return await Task.Run(() =>
				{
					using var compressedData = ArchiveFactory.Open(filePath);
					var fileList = new List<string>();
					foreach (var file in compressedData.Entries)
					{
						fileList.Add(file.Key);
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
