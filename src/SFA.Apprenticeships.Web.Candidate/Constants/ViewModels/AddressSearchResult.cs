namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using System.Collections.Generic;
    using Candidate.ViewModels.Locations;

    public class AddressSearchResult
    {
        public string ErrorMessage { get; set; }

        public bool HasError { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }
    }
}