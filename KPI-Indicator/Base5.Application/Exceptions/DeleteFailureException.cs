using System;

namespace Base5.Application.Exceptions
{
    public class DeleteFailureException : Exception
    {
        public DeleteFailureException(string message)
            : base($"{message}")
        {
        }
    }
}
