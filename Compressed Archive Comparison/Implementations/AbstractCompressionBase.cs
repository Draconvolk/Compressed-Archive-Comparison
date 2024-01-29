
using SharpCompress.Archives;

namespace CompressedArchiveComparison
{
	public abstract class AbstractCompressionBase: ICompression
	{
		protected AbstractCompressionBase()
		{
		}

		protected AbstractCompressionBase(string fileName)
		{
			FileName = fileName;
		}

		public virtual string FileName { get; set; } = "";

		public virtual async Task<IEnumerable<string>> GetFiles()
		{
			if (!string.IsNullOrWhiteSpace(FileName))
			{
				return await GetFiles(FileName);
			}
			else
			{
				return new List<string>();
			}
		}

		public virtual async Task<IEnumerable<string>> GetFiles(string filePath)
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

		public virtual string GetTypeName() => GetType().Name;
	}
}