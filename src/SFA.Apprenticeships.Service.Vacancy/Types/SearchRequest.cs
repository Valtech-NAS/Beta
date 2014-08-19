namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;
    using System.Collections.Generic;

    public class SearchRequest
    {
        public string Id { get; set; }

        public string Keywords { get; set; }

        public KeyValuePair<string, string>[] Parameters { get; set; }
    }
}
