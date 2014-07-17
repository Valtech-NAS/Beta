namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Templates.EditorFor
{
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    [ElementLocator(Id = "address-details")]
    public class AddressTemplate : WebElement
    {
        private IWebElement _uprn;

        public AddressTemplate(ISearchContext searchContext) : base(searchContext)
        {
        }

        [ElementLocator(Id = "Address_AddressLine1")]
        public IWebElement AddressLine1 { get; set; }

        [ElementLocator(Id = "Address_AddressLine2")]
        public IWebElement AddressLine2 { get; set; }

        [ElementLocator(Id = "Address_AddressLine3")]
        public IWebElement AddressLine3 { get; set; }

        [ElementLocator(Id = "Address_AddressLine4")]
        public IWebElement AddressLine4 { get; set; }

        [ElementLocator(Id = "Address_Postcode")]
        public IWebElement Postcode { get; set; }

        [ElementLocator(Id = "Address_Uprn")]
        public IWebElement Uprn
        {
            get { return _uprn; }
            set { _uprn = value; }
        }

        [ElementLocator(Id = "Address_GeoPoint_Latitude")]
        public IWebElement Latitude { get; set; }

        [ElementLocator(Id = "Address_GeoPoint_Longitude")]
        public IWebElement Longitude { get; set; }

    }
}
