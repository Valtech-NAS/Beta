
namespace SFA.Apprenticeships.Common.Entities.ReferenceData
{
    public class LocalAuthority : ILegacyReferenceData
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string County { get; set; }
        public string ShortName { get; set; }
    }
}
