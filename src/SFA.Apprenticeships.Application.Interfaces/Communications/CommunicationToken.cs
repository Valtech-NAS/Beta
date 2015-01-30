namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System;

    public class CommunicationToken
    {
        public CommunicationTokens Key { get; set; }

        public string Value { get; set; }

        public CommunicationToken(CommunicationTokens key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
