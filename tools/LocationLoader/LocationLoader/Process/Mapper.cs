using System;
using CsvHelper.Configuration;

namespace LocationLoader.Process
{
    /// <summary>
    /// mapper class for CSV mapping
    /// </summary>
    internal class Mapper : CsvClassMap<Location>
    {
        public Mapper()
        {
            Map(m => m.Name).Name("Town");
            Map(m => m.County).Name("County");
            Map(m => m.Country).Name("Country");
            Map(m => m.Latitude).Name("Latitude");
            Map(m => m.Longitude).Name("Longitude");
            Map(m => m.Postcode).Name("Postcode");
        }
    }
}
