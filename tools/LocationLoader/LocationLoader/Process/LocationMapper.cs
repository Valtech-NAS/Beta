using System;
using CsvHelper.Configuration;

namespace LocationLoader.Process
{
    /// <summary>
    /// mapper class for CSV mapping
    /// </summary>
    internal class LocationMapper : CsvClassMap<LocationData>
    {
        public LocationMapper()
        {
            Map(m => m.Name).Name("Town");
            Map(m => m.County).Name("County");
            Map(m => m.Latitude).Name("Latitude");
            Map(m => m.Longitude).Name("Longitude");
            Map(m => m.Type).Name("Type");
        }
    }
}
