using Data;

namespace Utils
{
    public class AppException : Exception
    {

        public StandardResult ExceptionResult { get; set; }

        public AppException(string message, int statusCode)
        {
            ExceptionResult = new StandardResult
            {
                Messages = new List<string> { message },
                StatusCode = statusCode,
                Success = false
            };
        }
    }
}