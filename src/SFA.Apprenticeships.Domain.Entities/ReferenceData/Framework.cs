namespace SFA.Apprenticeships.Domain.Entities.ReferenceData
{
    public class Framework
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public Occupation Occupation { get; set; }
    }
}