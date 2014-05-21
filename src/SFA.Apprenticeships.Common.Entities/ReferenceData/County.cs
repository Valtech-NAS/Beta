
using SFA.Apprenticeships.Common.Interfaces.ReferenceData;
using SFA.Apprenticeships.Common.Interfaces.Services;

namespace SFA.Apprenticeships.Common.Entities.ReferenceData
{
    public class County : ILegacyReferenceData
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
