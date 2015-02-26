
namespace SFA.Apprenticeships.Web.Employer.Mappers
{
    using SFA.Apprenticeships.Domain.Entities;
    using SFA.Apprenticeships.Web.Employer.Mappers.Interfaces;
    using SFA.Apprenticeships.Web.Employer.ViewModels;
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Common.Extensions;

    public class ReferenceDataMapper : IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel>, IViewModelToDomainMapper<ReferenceDataViewModel, ReferenceData>
    {
        public ReferenceDataViewModel ConvertToViewModel(ReferenceData domain)
        {
            domain.ThrowIfNull("ReferenceData", "domain object of type ReferenceData object can't be null");            

            return new ReferenceDataViewModel()
            {
                Id = domain.Id,
                Description = domain.Description
            };
        }
                 
        public ReferenceData ConvertToDomain(ReferenceDataViewModel viewModel)
        {
            viewModel.ThrowIfNull("ReferenceDataViewModel", "viewModel object of type ReferenceDataViewModel can't be null");

            return new ReferenceData()
            {
                Id = viewModel.Id,
                Description = viewModel.Description
            };
        }
    }
}