

namespace CompressedArchiveComparison.Exceptions
{
	public static class ExceptionList
	{
		private static readonly object AddLock = new();

		public static List<ICustomException> Exceptions { get; } = [];
		public static bool Any => Exceptions.Count != 0;

		public static void Add(Exception exception, string message = "", string location = "", List<string>? parameters = null)
		{
			var customEx = new CustomException(exception, message, location, parameters);
			lock (AddLock)
			{
				Exceptions.Add(customEx);
			}
#if DEBUG
			Console.WriteLine(message);
			Console.WriteLine(location);
			Console.WriteLine(parameters);
#endif
		}

		public static void DisplayExceptions()
		{
			if (!Any)
			{
				return;
			}
			lock (AddLock)
			{
				foreach (var ex in Exceptions)
				{
					Console.WriteLine("--------------------------------------------------");
					Console.WriteLine($"Message: {ex.Message}");
					Console.WriteLine($"Location: {ex.Location}");
					Console.WriteLine($"Params: ");
					if (ex.Params?.Count > 0)
					{
						foreach (var p in ex.Params)
						{
							Console.WriteLine($"\t{p}");
						}
					}
					if (ex.Exception != null)
					{
						Console.WriteLine($"Exception: {ex.Exception.Message}");
						if (ex.Exception.InnerException != null)
						{
							Console.WriteLine($"{ex.Exception.InnerException.Message}");
						}
					}
				}
			}
		}
	}
}
