namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    using System;

    public class CustomException : Exception
    {
        public CustomException(ErrorCodes code)
        {
            Code = code;
        }

        public CustomException(string message, ErrorCodes code)
            : base(message)
        {
            Code = code;
        }

        public CustomException(string format, ErrorCodes code, params object[] args)
            : base(string.Format(format, args))
        {
            Code = code;
        }

        public CustomException(string message, Exception innerException, ErrorCodes code)
            : base(message, innerException)
        {
            Code = code;
        }

        public CustomException(string format, Exception innerException, ErrorCodes code, params object[] args)
            : base(string.Format(format, args), innerException)
        {
            Code = code;
        }

        public ErrorCodes Code { get; private set; }
    }
}