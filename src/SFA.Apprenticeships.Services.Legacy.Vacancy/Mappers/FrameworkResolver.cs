using System;
using System.Linq;
using AutoMapper;
using SFA.Apprenticeships.Common.Interfaces.ReferenceData;

namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Mappers
{
    public class FrameworkResolver : ValueResolver<string, string>
    {
        private readonly IReferenceDataService _service;

        public FrameworkResolver(IReferenceDataService service)
        {
            _service = service;
        }

        protected override string ResolveCore(string source)
        {
            // TODO::need to use a cached service??
            var frameworks = _service.GetApprenticeshipFrameworks();

            var framework = frameworks.FirstOrDefault(x => x.Id.Equals(source, StringComparison.InvariantCultureIgnoreCase));
            return framework != null ? framework.Description : string.Empty;
        }
    }
}
