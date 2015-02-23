namespace SFA.Apprenticeships.Web.Employer.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using Domain.Enums;
    using Interfaces;
    using Mappers.Interfaces;
    using ViewModels;

    internal class EmployerEnquiryProvider : IEmployerEnquiryProvider
    {

        private ICommunciationService _communciationService;
        private IReferenceDataService _referenceDataService;
        //todo: move this to LocationMediator as it may be used by other places
        private ILocationSearchService _locationSearchService;
        private IDomainToViewModelMapper<Location, AddressViewModel> _locationDomainToViewModelMapper;
        private IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel> _referenceDataDomainToViewModelMapper;
        private IDomainToViewModelMapper<EmployerEnquiry, EmployerEnquiryViewModel> _employerEnquiryDomainToViewModelMapper;
        private IViewModelToDomainMapper<AddressViewModel, Location> _locationViewModelToDomainMapper;
        private IViewModelToDomainMapper<ReferenceDataViewModel, ReferenceData> _referenceDataViewModelToDomainMapper;
        private IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry> _employerEnquiryViewModelToDomainMapper;

        public EmployerEnquiryProvider(ICommunciationService communciationService, 
            IReferenceDataService referenceDataService, 
            ILocationSearchService locationSearchService,
            IDomainToViewModelMapper<Location, AddressViewModel> locationDtoVMapper,
            IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel> referenceDataDtoVMapper,
            IDomainToViewModelMapper<EmployerEnquiry, EmployerEnquiryViewModel> employerEnquiryDtoVMapper,
            IViewModelToDomainMapper<AddressViewModel, Location> locationVtoDMapper,
            IViewModelToDomainMapper<ReferenceDataViewModel, ReferenceData> referenceDataVtoDMapper,
            IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry> employerEnquiryVtoDMapper)
        {            
            _communciationService = communciationService;
            _referenceDataService = referenceDataService;
            _locationSearchService = locationSearchService;
            _locationDomainToViewModelMapper = locationDtoVMapper;
            _referenceDataDomainToViewModelMapper = referenceDataDtoVMapper;
            _employerEnquiryDomainToViewModelMapper = employerEnquiryDtoVMapper;
            _locationViewModelToDomainMapper = locationVtoDMapper;
            _referenceDataViewModelToDomainMapper = referenceDataVtoDMapper;
            _employerEnquiryViewModelToDomainMapper = employerEnquiryVtoDMapper;
        }

        public IEnumerable<AddressViewModel> FindAddress(string postcode)
        {
            //todo: try catch & return appropriate exceptions
            try
            {
                var addressData = _locationSearchService.FindAddress(postcode);

                var addressDataViewModel = addressData.Select(m => _locationDomainToViewModelMapper.ConvertToViewModel(m));
                return addressDataViewModel;
            }
            catch (System.Exception exception)
            {
                //todo: log error using preferred logging mechanism
                return Enumerable.Empty<AddressViewModel>();

            }
        }

        public IEnumerable<ReferenceDataViewModel> GetReferenceData(ReferenceDataTypes type)
        {
            try
            {
                var referenceData = _referenceDataService.Get(type);

                var referenceDataViewModel = referenceData.Select(m => _referenceDataDomainToViewModelMapper.ConvertToViewModel(m));

                return referenceDataViewModel;
            }
            catch (System.Exception)
            {
                //todo: log error using preferred logging mechanism
                return Enumerable.Empty<ReferenceDataViewModel>();
            }
        }

        public void SubmitEnquiry(EmployerEnquiryViewModel employerEnquiryData)
        {
            try
            {
                var employerEnquiry = _employerEnquiryViewModelToDomainMapper.ConvertToDomain(employerEnquiryData);
                _communciationService.SubmitEnquiry(employerEnquiry);
            }
            catch (System.Exception)
            {
                //todo: log error using preferred logging mechanism
                return;
            }
        }
    }
}