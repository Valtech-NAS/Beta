namespace SFA.Apprenticeships.Infrastructure.Communication.Configuration
{
    using System.Configuration;

    public class SendGridTemplateConfiguration : ConfigurationElement
    {
        private const string IdConstant = "Id";
        private const string NameConstant = "Name";

        [ConfigurationProperty(NameConstant, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameConstant]; }
            set { this[NameConstant] = value; }
        }

        [ConfigurationProperty(IdConstant, IsRequired = false, IsKey = false)]
        public string Id
        {
            get { return (string)this[IdConstant]; }
            set { this[IdConstant] = value; }
        }
    }
}
