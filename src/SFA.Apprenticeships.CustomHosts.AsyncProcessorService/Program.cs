namespace SFA.Apprenticeships.CustomHosts.AsyncProcessorService
{
    using System.ServiceProcess;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new AsyncProcessorService() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
