using System;

namespace SFA.Apprenticeships.Services.Models.ReferenceDataModels
{
    public class Region : ILegacyReferenceData
    {
        public string CodeName { get; set; }
        public string FullName { get; set; }
    }
}
