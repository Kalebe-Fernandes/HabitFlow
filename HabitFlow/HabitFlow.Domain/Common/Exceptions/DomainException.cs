namespace HabitFlow.Domain.Common.Exceptions
{
    public class DomainException : Exception
    {
        /// <summary>
        /// Gets the error code associated with this exception.
        /// </summary>
        public string ErrorCode { get; }

        public DomainException(string message, string errorCode = "DOMAIN_ERROR")
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public DomainException(string message, Exception innerException, string errorCode = "DOMAIN_ERROR")
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
