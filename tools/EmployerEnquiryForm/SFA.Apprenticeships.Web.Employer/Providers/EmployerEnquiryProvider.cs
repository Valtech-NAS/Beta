namespace SFA.Apprenticeships.Web.Employer.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Extensions;
    using Interfaces;
    using Mappers.Interfaces;
    using ViewModels;

    internal class EmployerEnquiryProvider : IEmployerEnquiryProvider
    {

        private ICommunciationService _communciationService;
        private IReferenceDataService _referenceDataService;
        private IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel> _referenceDataDomainToViewModelMapper;
        private IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry> _employerEnquiryViewModelToDomainMapper;

        public EmployerEnquiryProvider(ICommunciationService communciationService, 
            IReferenceDataService referenceDataService, 
            IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel> referenceDataDtoVMapper,
            IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry> employerEnquiryVtoDMapper)
        {            
            _communciationService = communciationService;
            _referenceDataService = referenceDataService;
            _referenceDataDomainToViewModelMapper = referenceDataDtoVMapper;
            _employerEnquiryViewModelToDomainMapper = employerEnquiryVtoDMapper;
        }

        public ReferenceDataListViewModel GetReferenceData(ReferenceDataTypes type)
        {
            try
            {
                var referenceData = _referenceDataService.Get(type);

                var referenceDataViewModel = referenceData.Select(m => _referenceDataDomainToViewModelMapper.ConvertToViewModel(m));
                IEnumerable<ReferenceDataViewModel> referenceDataViewModels = referenceDataViewModel as IList<ReferenceDataViewModel> ?? referenceDataViewModel.ToList();
                if (!referenceDataViewModels.IsNullOrEmpty())
                {
                    return new ReferenceDataListViewModel(referenceDataViewModels);
                }
                return new ReferenceDataListViewModel();
                
            }
            catch (System.Exception)
            {
                //todo: log error using preferred logging mechanism
                return new ReferenceDataListViewModel();
            }
        }

        public SubmitQueryStatus SubmitEnquiry(EmployerEnquiryViewModel employerEnquiryData)
        {
            try
            {
                var employerEnquiry = _employerEnquiryViewModelToDomainMapper.ConvertToDomain(employerEnquiryData);
                _communciationService.SubmitEnquiry(employerEnquiry);
                return SubmitQueryStatus.Success;
            }
            catch (System.Exception exception)
            {
                //todo: log error using preferred logging mechanism
                return SubmitQueryStatus.Error;
                //todo: add other cases if there are any
            }
        }
    }
}