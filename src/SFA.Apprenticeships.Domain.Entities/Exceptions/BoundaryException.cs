namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    using System;

    public class BoundaryException : CustomException
    {
        public BoundaryException(string code)
            : base(code)
        {
            Code = code;
        }

        public BoundaryException(string code, object data)
            : base(code)
        {
            Code = code;
            this.AddData(data);
        }

        public BoundaryException(string code, Exception innerException)
            : base(code, innerException, code)
        {
            Code = code;
        }
    }
}
