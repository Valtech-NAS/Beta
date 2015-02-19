namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    using System;

    //todo: 1.6: remove message arguments from exception ctors and replace with data items
    public class CustomException : Exception
    {
        public CustomException(string code)
        {
            Code = code;
        }

        public CustomException(string message, string code)
            : base(message)
        {
            Code = code;
        }

        public CustomException(string format, string code, params object[] args)
            : base(string.Format(format, args))
        {
            Code = code;
        }

        public CustomException(string message, Exception innerException, string code)
            : base(message, innerException)
        {
            Code = code;
        }

        public CustomException(string format, Exception innerException, string code, params object[] args)
            : base(string.Format(format, args), innerException)
        {
            Code = code;
        }

        public string Code { get; protected set; }
    }
}
