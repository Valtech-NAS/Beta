namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System.Collections.Generic;
    using ViewModels.Locations;
    using Application.Interfaces.Locations;
    using Domain.Interfaces.Mapping;
    using Domain.Entities.Locations;

    public class AddressSearchServiceProvider : IAddressSearchServiceProvider
    {
        private readonly IAddressSearchService _addressSearchService;
        private readonly IMapper _mapper;

        public AddressSearchServiceProvider(IMapper mapper, IAddressSearchService addressSearchService)
        {
            _mapper = mapper;
            _addressSearchService = addressSearchService;
        }

        public IEnumerable<AddressViewModel> FindAddress(string postcode)
        {
            var addresses = _addressSearchService.FindAddress(postcode);

            return addresses != null ? _mapper.Map<IEnumerable<Address>, IEnumerable<AddressViewModel>>(addresses) : new AddressViewModel[] {};
        }
    }
}