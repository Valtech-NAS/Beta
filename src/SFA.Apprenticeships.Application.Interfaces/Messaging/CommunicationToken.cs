namespace SFA.Apprenticeships.Application.Interfaces.Messaging
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
