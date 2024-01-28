namespace SkyrimModVerification
{
	public class CompressionFactory
	{
		public const string zip = ".zip";
		public const string sevenZ = ".7z";
		public const string rar = ".rar";

		/// <summary>
		/// Returns an appropriate compression object based on the fileName extension
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static ICompression? GetCompressionType(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName) || fileName.LastIndexOf('.') == -1)
			{
				Console.WriteLine($"*** Error Getting Compression Type, filename invalid: {fileName}");
				return null;
			}

			var type = fileName[fileName.LastIndexOf('.')..];
			return type switch
			{
				rar => new RarCompression(fileName),
				sevenZ => new SevenZipCompression(fileName),
				zip => new ZipCompression(fileName),
				_ => new ZipCompression(fileName)
			};
		}
	}
}
