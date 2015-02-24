namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(ContactMessageViewModelValidator))]
    public class ContactMessageViewModel
    {
        [Display(Name = ContactMessageViewModelMessages.FullNameMessages.LabelText)]
        public string Name { get; set; }

        [Display(Name = ContactMessageViewModelMessages.EmailAddressMessages.LabelText)]
        public string Email { get; set; }

        public string Enquiry { get; set; }

        [Display(Name = ContactMessageViewModelMessages.DetailsMessages.LabelText)]
        public string Details { get; set; }

        public SelectList Enquiries { get; set; }

        public string SelectedEnquiry { get; set; }
    }
}