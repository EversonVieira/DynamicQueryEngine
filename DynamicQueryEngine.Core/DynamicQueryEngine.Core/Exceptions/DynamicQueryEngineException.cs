using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DynamicQueryEngine.Core.Exceptions
{
    public class DynamicQueryEngineException : Exception
    {
        public DynamicQueryEngineException()
        {
        }

        public DynamicQueryEngineException(string? message) : base(message)
        {
        }

        public DynamicQueryEngineException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }

    public class DynamicQueryEngineArgumentNullException : DynamicQueryEngineException
    {
        public DynamicQueryEngineArgumentNullException(string? message) : base($"Argument '{message}' can't be null.")
        {

        }
        public DynamicQueryEngineArgumentNullException(string? message, Exception? innerException) : base($"Argument '{message}' can't be null.", innerException)
        {
        }
    }
}
