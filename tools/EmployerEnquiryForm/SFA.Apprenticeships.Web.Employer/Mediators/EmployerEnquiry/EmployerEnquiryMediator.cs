namespace SFA.Apprenticeships.Web.Employer.Mediators.EmployerEnquiry
{
    using System.Web.Mvc;
    using Constants;
    using Constants.Pages;
    using Domain.Enums;
    using Interfaces;
    using Providers.Interfaces;
    using Validators;
    using ViewModels;

    public class EmployerEnquiryMediator : MediatorBase, IEmployerEnquiryMediator
    {
        private IEmployerEnquiryProvider _employerEnquiryProvider;
        private EmployerEnquiryViewModelServerValidators _validators;

        public EmployerEnquiryMediator(IEmployerEnquiryProvider employerEnquiryProvider, EmployerEnquiryViewModelServerValidators validators)
        {
            _employerEnquiryProvider = employerEnquiryProvider;
            _validators = validators;
        }

        public MediatorResponse<ReferenceDataListViewModel> GetReferenceData(ReferenceDataTypes type)
        {
            var result = _employerEnquiryProvider.GetReferenceData(type);
            return GetMediatorResponse(EmployerEnquiryMediatorCodes.ReferenceData.Success, result);
        }

        public MediatorResponse<EmployerEnquiryViewModel> SubmitEnquiry()
        {
            var model = new EmployerEnquiryViewModel
            {
                //Get The various reference data list
                EmployeesCountList = GetEmployeeCountTypes(),
                WorkSectorList = GetWorkSectorTypes(),
                PreviousExperienceTypeList = GetPreviousExperienceTypes(),
                TitleList = GetTitleTypes(),
                EnquirySourceList = GetEnquirySourceTypes()
            };
            return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Success, model);
        }

        public MediatorResponse<EmployerEnquiryViewModel> SubmitEnquiry(EmployerEnquiryViewModel viewModel)
        {
            var validationResult = _validators.Validate(viewModel);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.ValidationError, viewModel, validationResult);
            }

            //todo: add other cases..
            SubmitQueryStatus resultStatus = _employerEnquiryProvider.SubmitEnquiry(viewModel);
            //populate reference data
            viewModel.EmployeesCountList = GetEmployeeCountTypes();
            viewModel.WorkSectorList = GetWorkSectorTypes();
            viewModel.PreviousExperienceTypeList = GetPreviousExperienceTypes();
            viewModel.EnquirySourceList = GetEnquirySourceTypes();
            viewModel.EmployeesCountList = GetEmployeeCountTypes();
            viewModel.TitleList = GetTitleTypes();

            switch (resultStatus)
            {
                case SubmitQueryStatus.Success:
                    return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Success, viewModel, EmployerEnquiryPageMessages.QueryHasBeenSubmittedSuccessfully, UserMessageLevel.Success);
                default:
                    return GetMediatorResponse(EmployerEnquiryMediatorCodes.SubmitEnquiry.Error, viewModel, EmployerEnquiryPageMessages.ErrorWhileQuerySubmission, UserMessageLevel.Error);
            }
        }


        private SelectList GetTitleTypes()
        {
            var employeeCountKeyValuePair = GetReferenceData(ReferenceDataTypes.Titles).ViewModel.ReferenceData;
            return new SelectList(employeeCountKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetEmployeeCountTypes()
        {
            var employeeCountKeyValuePair = GetReferenceData(ReferenceDataTypes.EmployeeCountTypes).ViewModel.ReferenceData;
            return new SelectList(employeeCountKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetWorkSectorTypes()
        {
            var employeeCountKeyValuePair = GetReferenceData(ReferenceDataTypes.WorkSectorTypes).ViewModel.ReferenceData;
            return new SelectList(employeeCountKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetPreviousExperienceTypes()
        {
            var employeeCountKeyValuePair = GetReferenceData(ReferenceDataTypes.PreviousExperienceTypes).ViewModel.ReferenceData;
            return new SelectList(employeeCountKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }

        private SelectList GetEnquirySourceTypes()
        {
            var employeeCountKeyValuePair = GetReferenceData(ReferenceDataTypes.EnquirySourceTypes).ViewModel.ReferenceData;
            return new SelectList(employeeCountKeyValuePair, CommonConstants.Id, CommonConstants.Description, string.Empty);
        }
    }
}