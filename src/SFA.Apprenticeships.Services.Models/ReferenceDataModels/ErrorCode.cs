using System;

namespace SFA.Apprenticeships.Services.Models.ReferenceDataModels
{
    public class ErrorCode : ILegacyReferenceData
    {
        public string CodeName { get; set; }
        public string FullName { get; set; }
    }
}
