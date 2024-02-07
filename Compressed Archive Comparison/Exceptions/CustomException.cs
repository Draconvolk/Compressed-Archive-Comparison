namespace CompressedArchiveComparison.Exceptions
{
	public class CustomException(Exception exception, string message = "", string location = "", List<string>? parameters = null) : ICustomException
	{
		public Exception Exception { get; set; } = exception;
		public string Message { get; set; } = message;
		public string Location { get; set; } = location;
		public List<string>? Params { get; set; } = parameters;
	}
}
