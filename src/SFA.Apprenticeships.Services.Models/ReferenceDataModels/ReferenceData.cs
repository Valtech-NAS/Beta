namespace SFA.Apprenticeships.Services.Models.ReferenceDataModels
{
    public abstract class ReferenceData
    {
        public string CodeName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
    }
}