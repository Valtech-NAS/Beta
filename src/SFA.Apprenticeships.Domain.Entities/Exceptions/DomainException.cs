namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    public class DomainException : CustomException
    {
        public DomainException(string code)
            : base(code)
        {
            Code = code;
        }

        public DomainException(string code, object data) : base(code)
        {
            Code = code;
            this.AddData(data);
        }

        public DomainException(string code, BoundaryException innerException, object data) : base(code, innerException, code)
        {
            Code = code;
            this.AddData(data);
        }
    }
}
