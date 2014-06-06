namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Configuration;

    public class ElasticsearchIndexConfiguration : ConfigurationElement, IElasticsearchIndexConfiguration
    {
        private const string NameConst = "Name";
        private const string MappingTypeConst = "MappingType";

        [ConfigurationProperty(NameConst, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameConst]; }
            set { this[NameConst] = value; }
        }

        [ConfigurationProperty(MappingTypeConst, IsRequired = true, IsKey = false)]
        [TypeConverter(typeof(TypeNameConverter))]
        public Type MappingType
        {
            get { return (Type)this[MappingTypeConst]; }
            set { this[MappingTypeConst] = value; }
        }
    }
}