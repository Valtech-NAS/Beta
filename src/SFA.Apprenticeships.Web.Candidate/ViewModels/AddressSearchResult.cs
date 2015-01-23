namespace SFA.Apprenticeships.Web.Candidate.ViewModels
{
    using System.Collections.Generic;
    using Locations;

    public class AddressSearchResult
    {
        public string ErrorMessage { get; set; }

        public bool HasError { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }
    }
}