using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Generators;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    public class RegisterAccount
    {
        protected readonly string _emailAddress;

        public RegisterAccount()
        {
            _emailAddress = EmailGenerator.GenerateEmailAddress();
        }
    }
}
