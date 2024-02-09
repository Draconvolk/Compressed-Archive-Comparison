
namespace CompressedArchiveComparison.Exceptions
{
    public interface IExceptionList
    {
        bool Any { get; }
        List<ICustomException> Exceptions { get; }

        /// <summary>
        /// Add a new record to the list
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="location"></param>
        /// <param name="parameters"></param>
        void Add(Exception exception, string message = "", string location = "", List<string>? parameters = null);
        /// <summary>
        /// Write out all records to the console
        /// </summary>
        void DisplayExceptions();
    }
}