namespace SFA.Apprenticeships.Infrastructure.Common.ActiveDirectory
{
    using System;
    using System.DirectoryServices.Protocols;
    using System.Text;

    public class ActiveDirectoryChangePassword
    {
        private const string LdapServerPolicyHintsOid = "1.2.840.113556.1.4.2066";
        private readonly ActiveDirectoryServer _server;

        public ActiveDirectoryChangePassword(ActiveDirectoryServer server)
        {
            _server = server;
        }

        public DirectoryResponse Change(
            string username, 
            string oldPassword = null,
            string newPassword = null, 
            bool enforceHistory = false)
        {
            var distinguishedName = string.Format(@"CN={0},{1}", username, _server.DistinguishedName);

            // the 'unicodePWD' attribute is used to handle pwd handling requests
            const string attribute = "unicodePwd";

            try
            {
                DirectoryAttributeModification[] damList;
                ModifyRequest modifyRequest;

                //do we have an old and a new pwd -> change pwd
                if (!String.IsNullOrEmpty(oldPassword) && !String.IsNullOrEmpty(newPassword))
                {
                    var directoryAttributeModificationDelete = new DirectoryAttributeModification
                    {
                        Name = attribute,
                        Operation = DirectoryAttributeOperation.Delete
                    };

                    directoryAttributeModificationDelete.Add(BuildBytePwd(oldPassword));

                    var directoryAttributeModificationAdd = new DirectoryAttributeModification
                    {
                        Name = attribute,
                        Operation = DirectoryAttributeOperation.Add
                    };

                    directoryAttributeModificationAdd.Add(BuildBytePwd(newPassword));

                    damList = new[] {directoryAttributeModificationDelete, directoryAttributeModificationAdd};
                    modifyRequest = new ModifyRequest(distinguishedName, damList);

                    return _server.Connection.SendRequest(modifyRequest);
                }

                if (!String.IsNullOrEmpty(newPassword))
                {
                    // do we have a pwd to set -> set pwd
                    var directoryAttributeModificationReplace = new DirectoryAttributeModification
                    {
                        Name = attribute,
                        Operation = DirectoryAttributeOperation.Replace
                    };

                    directoryAttributeModificationReplace.Add(BuildBytePwd(newPassword));

                    damList = new[] {directoryAttributeModificationReplace};
                    modifyRequest = new ModifyRequest(distinguishedName, damList);

                    // should we utilize pwd history on the pwd reset?
                    if (enforceHistory)
                    {
                        byte[] value = BerConverter.Encode("{i}", new object[] {0x1});
                        var pwdHistory = new DirectoryControl(LdapServerPolicyHintsOid, value, false, true);
                        modifyRequest.Controls.Add(pwdHistory);
                    }

                    return _server.Connection.SendRequest(modifyRequest);
                }
            }
            catch (DirectoryOperationException doex)
            {
                // TODO::Low::Act on the exceptions
                switch (doex.Response.ResultCode)
                {
                    case ResultCode.UnwillingToPerform:
                        //Console.WriteLine("Pwd violates pwd-history: {0}", doex.Response.ErrorMessage);
                        break;

                    case ResultCode.ConstraintViolation:
                        //Console.WriteLine("Pwd constraints: {0}", doex.Response.ErrorMessage);
                        break;

                    //default:
                    //    //Console.WriteLine("Update pwd error: {0}", doex.Response.ErrorMessage);
                    //    break;
                }

                throw;
            }

            return null;
        }

        private static byte[] BuildBytePwd(string pwd)
        {
            return (Encoding.Unicode.GetBytes(String.Format("\"{0}\"", pwd)));
        }
    }
}
