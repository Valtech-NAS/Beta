namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using System.ServiceModel;
    using Types;

    [ServiceContract]
    public interface IVacancySearchService
    {
        [OperationContract]
        SearchResponse Search(SearchRequest request);
    }
}
