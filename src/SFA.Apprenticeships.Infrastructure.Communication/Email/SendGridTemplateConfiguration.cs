namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System.Configuration;

    public class SendGridTemplateConfiguration : ConfigurationElement
    {
        private const string IdConstant = "Id";
        private const string NameConstant = "Name";
        private const string FromEmailConstant = "FromEmail";

        [ConfigurationProperty(NameConstant, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameConstant]; }
            set { this[NameConstant] = value; }
        }

        [ConfigurationProperty(IdConstant, IsRequired = true, IsKey = false)]
        public string Id
        {
            get { return (string)this[IdConstant]; }
            set { this[IdConstant] = value; }
        }

        [ConfigurationProperty(FromEmailConstant, IsRequired = true, IsKey = false)]
        public string FromEmail
        {
            get { return (string)this[FromEmailConstant]; }
            set { this[FromEmailConstant] = value; }
        }
    }
}
