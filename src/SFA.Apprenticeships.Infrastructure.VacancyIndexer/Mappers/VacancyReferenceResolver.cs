namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers
{
    using AutoMapper;

    public class VacancyReferenceResolver : ValueResolver<string, int>
    {
        protected override int ResolveCore(string source)
        {
            int vacancyReference;
            if(!string.IsNullOrEmpty(source) && int.TryParse(source, out vacancyReference))
                return vacancyReference;
            return 0;
        }
    }
}