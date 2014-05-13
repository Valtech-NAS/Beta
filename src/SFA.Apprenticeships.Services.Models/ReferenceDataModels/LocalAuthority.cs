using System;

namespace SFA.Apprenticeships.Services.Models.ReferenceDataModels
{
    public class LocalAuthority : ILegacyReferenceData
    {
        public string CodeName { get; set; }
        public string FullName { get; set; }
        public string County { get; set; }
        public string ShortName { get; set; }
    }
}
