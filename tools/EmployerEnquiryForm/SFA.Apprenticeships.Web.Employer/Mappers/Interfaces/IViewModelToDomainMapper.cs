namespace SFA.Apprenticeships.Web.Employer.Mappers.Interfaces
{
    using System.Collections.Generic;

    public interface IViewModelToDomainMapper<TSource, TDestination>
    {
        TDestination ConvertToDomain(TSource viewModel);                
    }
}