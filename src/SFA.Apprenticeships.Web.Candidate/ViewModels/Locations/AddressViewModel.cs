namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Locations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(AddressViewModelValidator))]
    public class AddressViewModel
    {
        [Display(Name = AddressMessages.AddressLine1.LabelText)]
        public string AddressLine1 { get; set; }

        [Display(Name = AddressMessages.AddressLine2.LabelText)]
        public string AddressLine2 { get; set; }

        [Display(Name = AddressMessages.AddressLine3.LabelText)]
        public string AddressLine3 { get; set; }

        [Display(Name = AddressMessages.AddressLine4.LabelText)]
        public string AddressLine4 { get; set; }

        [Display(Name = AddressMessages.Postcode.LabelText)]
        public string Postcode { get; set; }
        public string Uprn { get; set; }
        public GeoPointViewModel GeoPoint { get; set; }
    }
}