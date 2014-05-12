using System;

namespace SFA.Apprenticeships.Services.Models.ReferenceDataModels
{
    public class Framework : ILegacyReferenceData
    {
        public string ShortName { get; set; }
        public string CodeName { get; set; }
        public string FullName { get; set; }
    }
}
