namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Configuration;

    public class TwilioTemplateConfiguration : ConfigurationElement
    {
        private const string NameConstant = "Name";
        private const string MessageConstant = "Message";

        [ConfigurationProperty(NameConstant, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameConstant]; }
            set { this[NameConstant] = value; }
        }

        [ConfigurationProperty(MessageConstant, IsRequired = true, IsKey = false)]
        public string Message
        {
            get { return (string)this[MessageConstant]; }
            set { this[MessageConstant] = value; }
        }
    }
}
