namespace SFA.Apprenticeships.CustomHosts.AsyncProcessorService
{
    using System.ServiceProcess;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(params string[] parameters)
        {
            if (parameters.Length > 0 && parameters[0].ToLower() == "/c")
            {
                new AsyncProcessorService().RunConsole();
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                {
                    new AsyncProcessorService()
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
