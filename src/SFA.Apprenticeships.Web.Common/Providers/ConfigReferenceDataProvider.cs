using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using SFA.Apprenticeships.Web.Common.Models.Common;

namespace SFA.Apprenticeships.Web.Common.Providers
{
    /// <summary>
    /// Reference data service that uses the local config file for providing reference data
    /// </summary>
    public class ConfigReferenceDataProvider : IReferenceDataProvider
    {
        private readonly Lazy<XDocument> _data;

        public ConfigReferenceDataProvider()
        {
            _data = new Lazy<XDocument>(
                () =>
                {
                    var dataFilename = HttpContext.Current.Server.MapPath("~/App_Data/Lookups.xml");

                    if (!File.Exists(dataFilename))
                    {
                        throw new FileNotFoundException(string.Format("Reference data file not found ({0})", dataFilename));
                    }

                    return XDocument.Load(dataFilename);
                });
        }

        public IEnumerable<ReferenceDataViewModel> Get(Enum type)
        {
            return GetReferenceData(type.ToString());
        }

        #region Helpers
        private IEnumerable<ReferenceDataViewModel> GetReferenceData(string key)
        {
            var referenceData = _data.Value.XPathSelectElement("/referenceData");

            if (referenceData == null)
            {
                throw new InvalidOperationException("Reference data section missing from config file");
            }

            var items = from data in referenceData.Descendants("data")
                where (string) data.Attribute("key") == key
                from item in data.Descendants("item")
                select new ReferenceDataViewModel
                {
                    Id = item.Attribute("id").Value,
                    Description = item.Attribute("value").Value
                };

            return items.ToList();
        }
        #endregion
    }
}
