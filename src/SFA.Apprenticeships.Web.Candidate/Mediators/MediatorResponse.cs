namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System.Collections.Generic;

    public class MediatorResponse<T>
    {
        public string Code { get; set; }

        public T ViewModel { get; set; }

        public MediatorResponseMessage Message { get; set; }

        public IDictionary<string, string> Parameters { get; set; } 
    }
}