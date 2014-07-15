
namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System.Collections.Generic;
    using ViewModels.Locations;

    public interface IAddressSearchServiceProvider
    {
        IEnumerable<AddressViewModel> FindAddress(string postcode);
    }
}