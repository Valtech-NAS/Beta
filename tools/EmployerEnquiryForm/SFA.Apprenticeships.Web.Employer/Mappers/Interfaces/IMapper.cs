namespace SFA.Apprenticeships.Web.Employer.Mappers.Interfaces
{
    using System.Collections.Generic;

    public interface IMapper
    {
        TK ConvertToDomain<T, TK>(T viewModel);
        TK ConvertToViewModel<T, TK>(T domain);
        IEnumerable<TK> ConvertToDomain<T, TK>(IEnumerable<T> viewModel);
        IEnumerable<TK> ConvertToViewModel<T, TK>(IEnumerable<T> domain);
    }
}