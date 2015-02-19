namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    using System;

    public class BoundaryException : Exception
    {
        public BoundaryException(string code)
        {
            Code = code;
        }

        public BoundaryException(string code, Exception innerException) : base(code, innerException)
        {
            Code = code;
        }

        public string Code { get; private set; }
    }
}
