namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    using System;

    //todo: 1.6: remove message arguments from exception ctors and replace with data items
    public sealed class CustomException : Exception
    {
        public CustomException(string code)
        {
            //StoreData(code);
            Code = code;
        }

        public CustomException(string message, string code)
            : base(message)
        {
            //StoreData(code);
            Code = code;
        }

        public CustomException(string format, string code, params object[] args)
            : base(string.Format(format, args))
        {
            //StoreData(code);
            Code = code;
        }

        public CustomException(string message, Exception innerException, string code)
            : base(message, innerException)
        {
            //StoreData(code);
            Code = code;
        }

        public CustomException(string format, Exception innerException, string code, params object[] args)
            : base(string.Format(format, args), innerException)
        {
            //StoreData(code);
            Code = code;
        }

        public string Code { get; private set; }

        private void StoreData(Guid? candidateId = null, string candidateEmail = null, Guid? applicationId = null, int? vacancyId = null)
        {
            //todo: 1.6: add contextual information (if present) to the data items
            //Data.Add("candidate.id", candidateId);
            //Data.Add("candidate.email", candidateEmail);
            //Data.Add("application.id", applicationId);
            //Data.Add("vacancy.id", vacancyId);
        }
    }
}
