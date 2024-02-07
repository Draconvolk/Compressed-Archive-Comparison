using CompressedArchiveComparison.Compressions;
using CompressedArchiveComparison.Exceptions;

namespace CompressedArchiveComparison.Components
{
	public static class CompressionFactory
	{
		private static readonly string CF = "CompressionFactory";

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
				ExceptionList.Add(new Exception("Get compression failed. Invalid filename"), $"Can't get compression type due to an invalid filename {fileName}", $"{CF}\\GetCompressionType", [fileName]);
				return null;
			}
			try
			{
				var type = fileName[fileName.LastIndexOf('.')..];
				var compression = resolverAccessor(type);
				compression.SetFileName(fileName);
				return compression;
			}
			catch (Exception ex)
			{
				ExceptionList.Add(ex, $"Something went wrong trying to get the compression type from filename {fileName}", $"{CF}\\GetCompressionType", [fileName]);
				return null;
			}
		}
	}
}
