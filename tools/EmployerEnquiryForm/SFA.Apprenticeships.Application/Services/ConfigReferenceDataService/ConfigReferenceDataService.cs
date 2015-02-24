namespace SFA.Apprenticeships.Application.Services.ConfigReferenceDataService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Common.AppSettings;
    using Domain.Entities;
    using Domain.Enums;
    using Interfaces;

    public class ConfigReferenceDataService : IReferenceDataService
    {
        private readonly Lazy<XDocument> _data;

        public ConfigReferenceDataService()
        {
            _data = new Lazy<XDocument>(() =>
            {
                string dataFilename = BaseAppSettingValues.IsReferenceDataFileInDataDirectory
                    ? Path.Combine(Convert.ToString(AppDomain.CurrentDomain.GetData("DataDirectory")),
                        BaseAppSettingValues.ReferenceDataPathWithFile)
                    : BaseAppSettingValues.ReferenceDataPathWithFile;


                if (!File.Exists(dataFilename))
                    throw new Exception(string.Format("Reference data file not found ({0})", dataFilename));

                return XDocument.Load(dataFilename);
            });
        }

        public IEnumerable<ReferenceData> Get(ReferenceDataTypes type)
        {
            var data = GetReferenceData(type.ToString());
            return data;
        }

        #region Helpers
        private IEnumerable<ReferenceData> GetReferenceData(string key)
        {
            var referenceData = _data.Value.XPathSelectElement("/referenceData");

            if (referenceData == null)
                throw new Exception("Reference data section missing from config file");

            var items = from data in referenceData.Descendants("data")
                        where (string)data.Attribute("key") == key
                        from item in data.Descendants("item")
                        select new ReferenceData
                        {
                            Id = item.Attribute("id").Value,
                            Description = item.Attribute("value").Value
                        };

            return items.ToList();
        }
        #endregion
    }
}