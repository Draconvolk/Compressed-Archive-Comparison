namespace CompressedArchiveComparison.Exceptions
{
    public class ExceptionList(bool testingOnly = false) : IExceptionList
    {
        private readonly object AddLock = new();

        public List<ICustomException> Exceptions { get; } = [];
        public bool Any => Exceptions.Count != 0;

        public void Add(Exception exception, string message = "", string location = "", List<string>? parameters = null)
        {
            var customEx = new CustomException(exception, message, location, parameters);

            if (!testingOnly)
            {
                lock (AddLock)
                {
                    Exceptions.Add(customEx);
                }
            }
            else
            {
                DisplaySingle(customEx);
            }
        }

        public void DisplayExceptions()
        {
            if (!Any)
            {
                return;
            }
            lock (AddLock)
            {
                foreach (var ex in Exceptions)
                {
                    DisplaySingle(ex);
                }
            }
        }

        /// <summary>
        /// Write out a single record to the console
        /// </summary>
        /// <param name="ex"></param>
        public void DisplaySingle(ICustomException ex)
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
