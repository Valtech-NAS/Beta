namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System.Collections.Generic;
    using FluentValidation.Results;

    public class MediatorResponse<T>
    {
        public string Code { get; set; }

        public T ViewModel { get; set; }

        public MediatorResponseMessage Message { get; set; }

        public object Parameters { get; set; }

        public ValidationResult ValidationResult { get; set; }
    }
}