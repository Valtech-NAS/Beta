namespace TestUserCreator
{
    public class ActiveDirectoryConfig
    {
        public ActiveDirectoryConfig(
            string server,
            string distinguishedName,
            int port,
            string username,
            string password)
        {
            Server = server;
            DistinguishedName = distinguishedName;
            Port = port;
            Username = username;
            Password = password;
        }

        public string DistinguishedName { get; private set; }

        public string Server { get; private set; }
            
        public int Port { get; private set; }

        public string Username { get; private set; }
            
        public string Password { get; private set; }
    }
}
