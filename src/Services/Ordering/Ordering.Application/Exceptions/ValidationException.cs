using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException() : base("One or more validation exceptions have occured.")
        {
            Errors = new();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
