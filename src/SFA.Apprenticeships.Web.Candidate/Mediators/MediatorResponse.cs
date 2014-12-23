﻿namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System.Collections.Generic;
    using ViewModels;

    public class MediatorResponse<T>
        where T : ViewModelBase
    {
        public T ViewModel { get; set; }

        public string Code { get; set; }

        public MediatorResponseMessage Message { get; set; }

        public IDictionary<string, string> Parameters { get; set; } 
    }
}