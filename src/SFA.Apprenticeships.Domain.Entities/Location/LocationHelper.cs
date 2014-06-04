namespace SFA.Apprenticeships.Domain.Entities.Location
{
    using System;

    public class LocationHelper
    {
        const string PostcodeRegex = "TODO"; // http://regexlib.com/REDetails.aspx?regexp_id=2653

        public static bool IsPostcode(string postcode)
        {
            return true; //todo: use regex to see if the postcode passed in, need unit tests
        }
    }
}
