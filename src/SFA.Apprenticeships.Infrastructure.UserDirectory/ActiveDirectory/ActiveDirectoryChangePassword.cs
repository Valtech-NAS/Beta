namespace SFA.Apprenticeships.Infrastructure.UserDirectory.ActiveDirectory
{
    using System;
    using System.DirectoryServices.Protocols;
    using System.Text;
    using Configuration;
    using Domain.Entities.Exceptions;
    using NLog;

    public class ActiveDirectoryChangePassword
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ActiveDirectoryConfiguration _config;
        private readonly ActiveDirectoryServer _server;

        public ActiveDirectoryChangePassword(ActiveDirectoryServer server, ActiveDirectoryConfiguration config)
        {
            _server = server;
            _config = config;
        }

        public DirectoryResponse Change(
            string username,
            string oldPassword = null,
            string newPassword = null)
        {
            Logger.Debug("Calling active directory server to set or change password username={0}.", username);

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                Logger.Debug("New password is empty, active directory server call cannot continue further.");
                throw new CustomException("New password cannot be empty", ErrorCodes.LdapEmptyNewPasswordError);
            }

            var distinguishedName = string.Format(@"CN={0},{1}", username, _server.DistinguishedName);
            var attribute = _config.DirectoryAttributeName;

            try
            {
                ModifyRequest modifyRequest;

                if (!string.IsNullOrWhiteSpace(oldPassword))
                {
                    Logger.Debug(
                        "Generating change password modification request for username={0} with distinguishedName={1}",
                        username, distinguishedName);
                    modifyRequest = GenerateChangePasswordModifyRequest(oldPassword, newPassword, attribute,
                        distinguishedName);
                }
                else
                {
                    Logger.Debug(
                        "Generating set password modification request for username={0} with distinguishedName={1}",
                        username, distinguishedName);
                    modifyRequest = GenerateSetPasswordModifyRequest(newPassword, attribute, distinguishedName);
                }

                Logger.Debug("Sending generated request to server for username={0}", username);

                return _server.Connection.SendRequest(modifyRequest);
            }
            catch (DirectoryOperationException e)
            {
                Logger.Error("DirectoryOperationException with the following details was thrown: ", e);
                throw new CustomException("Password modify request failed", e, ErrorCodes.LdapModifyPasswordError);
            }
        }

        #region Helpers

        private static ModifyRequest GenerateSetPasswordModifyRequest(string newPassword, string attribute,
            string distinguishedName)
        {
            var directoryAttributeModificationReplace = new DirectoryAttributeModification
            {
                Name = attribute,
                Operation = DirectoryAttributeOperation.Replace
            };

            directoryAttributeModificationReplace.Add(BuildBytePwd(newPassword));

            DirectoryAttributeModification[] directoryAttributeModifications = { directoryAttributeModificationReplace };
            var modifyRequest = new ModifyRequest(distinguishedName, directoryAttributeModifications);
            return modifyRequest;
        }

        private static ModifyRequest GenerateChangePasswordModifyRequest(string oldPassword, string newPassword,
            string attribute, string distinguishedName)
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

            DirectoryAttributeModification[] directoryAttributeModifications =
            {
                directoryAttributeModificationDelete,
                directoryAttributeModificationAdd
            };

            var modifyRequest = new ModifyRequest(distinguishedName, directoryAttributeModifications);

            return modifyRequest;
        }

        private static byte[] BuildBytePwd(string pwd)
        {
            return (Encoding.Unicode.GetBytes(String.Format("\"{0}\"", pwd)));
        }

        #endregion
    }
}