namespace SFA.Apprenticeships.Web.Employer.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants;
    using Constants.Pages;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(EmployerEnquiryViewModelClientValidator))]
    public class EmployerEnquiryViewModel
    {
        [Display(Name = EmployerEnquiryViewModelMessages.NameTitleMessages.LabelText)]
        public string Title { get; set; }
        public SelectList TitleList { get; set; }

        [Display(Name = EmployerEnquiryViewModelMessages.EmailAddressMessages.LabelText)]
        public string Email { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.FirstnameMessages.LabelText)]
        public string Firstname { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.LastnameMessages.LabelText)]
        public string Lastname { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.CompanynameMessages.LabelText)]
        public string Companyname { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.PositionMessages.LabelText)]
        public string Position { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.WorkPhoneNumberMessages.LabelText, Description = EmployerEnquiryViewModelMessages.WorkPhoneNumberMessages.HintText)]
        public string WorkPhoneNumber { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.MobileNumberMessages.LabelText)]
        public string MobileNumber { get; set; }
        public SelectList EmployeesCountList { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.EmployeCountMessages.LabelText)]
        public string EmployeesCount { get; set; }
        public AddressViewModel Address { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.WorkSectorMessages.LabelText)]
        public string WorkSector { get; set; }
        public SelectList WorkSectorList { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.PreviousExperienceTypeMessages.LabelText, Description = EmployerEnquiryViewModelMessages.PreviousExperienceTypeMessages.HintText)]
        public string PreviousExperienceType { get; set; }
        public SelectList PreviousExperienceTypeList { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.EnquiryDescriptionMessages.LabelText)]
        public string EnquiryDescription { get; set; }
        [Display(Name = EmployerEnquiryViewModelMessages.EnquirySourceMessages.LabelText)]
        public string EnquirySource { get; set; }
        public SelectList EnquirySourceList { get; set; }
    }
}