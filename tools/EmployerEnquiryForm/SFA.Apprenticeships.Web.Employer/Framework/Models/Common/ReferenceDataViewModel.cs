namespace SFA.Apprenticeships.Web.Employer.Framework.Models.Common
{
    using System.ComponentModel;

    public class ReferenceDataViewModel
    {
        [DisplayName("Code")]
        public string Id { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
