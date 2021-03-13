using System;
using System.Diagnostics.CodeAnalysis;

namespace Hermes.API.User.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ServiceArgumentNullException : ArgumentNullException
    {
        private ExceptionCodes _exceptionCode;

        public ServiceArgumentNullException(string paramName, ExceptionCodes exceptionCode) : base(paramName)
        {
            _exceptionCode = exceptionCode;
        }

        public ServiceArgumentNullException(string paramName, ExceptionCodes exceptionCode, Exception innerException) :
            base(paramName, innerException)
        {
            _exceptionCode = exceptionCode;
        }

        public ServiceArgumentNullException(string message, Exception innerException, ExceptionCodes exceptionCode) :
            base(message, innerException)
        {
            _exceptionCode = exceptionCode;
        }

        public ServiceArgumentNullException(string paramName, string message, ExceptionCodes exceptionCode) : base(
            paramName, message)
        {
            _exceptionCode = exceptionCode;
        }
    }
}