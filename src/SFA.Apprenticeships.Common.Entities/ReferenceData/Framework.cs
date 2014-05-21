
using SFA.Apprenticeships.Common.Interfaces.ReferenceData;
using SFA.Apprenticeships.Common.Interfaces.Services;

namespace SFA.Apprenticeships.Common.Entities.ReferenceData
{
    public class Framework : ILegacyReferenceData
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public Occupation Occupation { get; set; }
    }
}