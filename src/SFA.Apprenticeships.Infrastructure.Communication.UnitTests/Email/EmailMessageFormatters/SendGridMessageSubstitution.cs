namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters
{
    using System.Collections.Generic;

    public class SendGridMessageSubstitution
    {
        public SendGridMessageSubstitution(string replacementTag, List<string> substitutionValues)
        {
            ReplacementTag = replacementTag;
            SubstitutionValues = substitutionValues;
        }

        public string ReplacementTag { get; private set; }

        public List<string> SubstitutionValues { get; private set; } 
    }
}