namespace HabitFlow.Domain.Common.Exceptions
{
    public class ValidationException : DomainException
    {
        /// <summary>
        /// Gets the collection of validation errors.
        /// </summary>
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ValidationException(string message) : base(message, "VALIDATION_ERROR")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message, IDictionary<string, string[]> errors) : base(message, "VALIDATION_ERROR")
        {
            Errors = new Dictionary<string, string[]>(errors);
        }

        public ValidationException(string propertyName, string errorMessage) : base(errorMessage, "VALIDATION_ERROR")
        {
            Errors = new Dictionary<string, string[]>
            {
                { propertyName, new[] { errorMessage } }
            };
        }
    }
}
