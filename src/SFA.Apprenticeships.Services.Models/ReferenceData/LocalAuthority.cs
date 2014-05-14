
namespace SFA.Apprenticeships.Services.Models.ReferenceData
{
    public class LocalAuthority : ILegacyReferenceData
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string County { get; set; }
        public string ShortName { get; set; }
    }
}
