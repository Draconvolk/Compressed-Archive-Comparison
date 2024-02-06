using CompressedArchiveComparison.Compressions;

namespace CompressedArchiveComparison.Components
{
	public static class CompressionFactory
	{
		public const string zip = ".zip";
		public const string sevenZ = ".7z";
		public const string rar = ".rar";

		/// <summary>
		/// Returns an appropriate compression object based on the fileName extension
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static ICompression? GetCompressionType(CompressionResolver resolverAccessor, string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName) || fileName.LastIndexOf('.') == -1)
			{
				Console.WriteLine($"*** Error Getting Compression Type, filename invalid: {fileName}");
				return null;
			}

			var type = fileName[fileName.LastIndexOf('.')..];
			var compression = resolverAccessor(type);
			compression.SetFileName(fileName);
			return compression;
		}
	}
}
