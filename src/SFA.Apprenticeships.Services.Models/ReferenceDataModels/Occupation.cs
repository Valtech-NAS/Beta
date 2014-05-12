using System;

namespace SFA.Apprenticeships.Services.Models.ReferenceDataModels
{
    public class Occupation : ILegacyReferenceData
    {
        public string ShortName { get; set; }
        public string CodeName { get; set; }
        public string FullName { get; set; }
    }
}
