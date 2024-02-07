namespace CompressedArchiveComparison.Exceptions
{
	public interface ICustomException
	{
		public Exception Exception { get; set; }
		public string Message { get; set; }
		public string Location { get; set; }
		public List<string>? Params { get; set; }
	}
}
