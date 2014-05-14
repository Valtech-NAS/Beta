
namespace SFA.Apprenticeships.Services.Models.ReferenceData
{
    public class Framework : ILegacyReferenceData
    {
        public string ShortName { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public Occupation Occupation { get; set; }
    }
}