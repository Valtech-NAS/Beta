namespace SFA.Apprenticeships.Web.Common.Models.Common
{
    using System;
    using System.ComponentModel;

    public class ReferenceDataViewModel
    {
        [DisplayName("Code")]
        public string Id { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
