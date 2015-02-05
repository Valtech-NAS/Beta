namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    using System;

    public sealed class CustomException : Exception
    {
        public CustomException(string code)
        {
            StoreData(code);
            Code = code;
        }

        public CustomException(string message, string code)
            : base(message)
        {
            StoreData(code);
            Code = code;
        }

        public CustomException(string format, string code, params object[] args)
            : base(string.Format(format, args))
        {
            StoreData(code);
            Code = code;
        }

        public CustomException(string message, Exception innerException, string code)
            : base(message, innerException)
        {
            StoreData(code);
            Code = code;
        }

        public CustomException(string format, Exception innerException, string code, params object[] args)
            : base(string.Format(format, args), innerException)
        {
            StoreData(code);
            Code = code;
        }

        public string Code { get; private set; }

        private void StoreData(string code)
        {
            Data.Add("error_code", code); //todo: temp code, will be handled in log handler
        }
    }
}
