
namespace SFA.Apprenticeships.Web.Employer.Mappers
{
    using SFA.Apprenticeships.Domain.Entities;
    using SFA.Apprenticeships.Web.Employer.Mappers.Interfaces;
    using SFA.Apprenticeships.Web.Employer.ViewModels;
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Web.Employer.Extensions;

    public class ReferenceDataMapper : IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel>, IViewModelToDomainMapper<ReferenceDataViewModel, ReferenceData>
    {
        public ReferenceDataViewModel ConvertToViewModel(ReferenceData domain)
        {
            domain.ThrowIfNull<ReferenceData>("ReferenceData", "ReferenceData object can't be null");            

            return new ReferenceDataViewModel()
            {
                Key = domain.Id,
                Value = domain.Description
            };
        }
                 
        public ReferenceData ConvertToDomain(ReferenceDataViewModel viewModel)
        {
            viewModel.ThrowIfNull <ReferenceDataViewModel>("ReferenceDataViewModel", "ReferenceDataViewModel can't be null");

            return new ReferenceData()
            {
                Id = viewModel.Key,
                Description = viewModel.Value
            };
        }
    }
}