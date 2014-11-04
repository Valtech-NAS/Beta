namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Locations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FluentValidation.Attributes;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.Validators;

    [Serializable]
    [Validator(typeof (AddressViewModelValidator))]
    public class AddressViewModel
    {
        private string _postcode;

        [Display(Name = AddressViewModelMessages.AddressLine1.LabelText)]
        public string AddressLine1 { get; set; }

        [Display(Name = AddressViewModelMessages.AddressLine2.LabelText)]
        public string AddressLine2 { get; set; }

        [Display(Name = AddressViewModelMessages.AddressLine3.LabelText)]
        public string AddressLine3 { get; set; }

        [Display(Name = AddressViewModelMessages.AddressLine4.LabelText)]
        public string AddressLine4 { get; set; }

        [Display(Name = AddressViewModelMessages.Postcode.LabelText)]
        public string Postcode
        {
            get { return string.IsNullOrWhiteSpace(_postcode) ? _postcode : _postcode.ToUpperInvariant(); }
            set { _postcode = !string.IsNullOrWhiteSpace(value) ? value.ToUpperInvariant() : value; }
        }

        public string Uprn { get; set; }
        public GeoPointViewModel GeoPoint { get; set; }
    }
}