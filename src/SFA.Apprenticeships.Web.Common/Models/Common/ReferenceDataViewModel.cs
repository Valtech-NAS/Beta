using System;
using System.ComponentModel;

namespace SFA.Apprenticeships.Web.Common.Models.Common
{
    public class ReferenceDataViewModel
    {
        public const string Unknown = "";

        [DisplayName("Code")]
        public string Id { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
