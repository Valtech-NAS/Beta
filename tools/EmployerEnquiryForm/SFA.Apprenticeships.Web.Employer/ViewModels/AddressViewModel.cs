namespace SFA.Apprenticeships.Web.Employer.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants;
    using FluentValidation.Attributes;
    using Validators;
    
    [Validator(typeof(AddressViewModelValidators))]
    public class AddressViewModel
    {
        private string _postcode;
        [Display(Name = AddressViewModelMessages.AddressLine1Messages.LabelText)]
        public string AddressLine1 { get; set; }
        [Display(Name = AddressViewModelMessages.AddressLine2Messages.LabelText)]
        public string AddressLine2 { get; set; }
        [Display(Name = AddressViewModelMessages.AddressLine3Messages.LabelText)]
        public string AddressLine3 { get; set; }
        [Display(Name = AddressViewModelMessages.CityMessages.LabelText)]
        public string City { get; set; }
        [Display(Name = AddressViewModelMessages.PostcodeMessages.LabelText)]
        public string Postcode
        {
            get { return string.IsNullOrWhiteSpace(_postcode) ? _postcode : _postcode.ToUpperInvariant(); }
            set { _postcode = !string.IsNullOrWhiteSpace(value) ? value.ToUpperInvariant() : value; }
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}