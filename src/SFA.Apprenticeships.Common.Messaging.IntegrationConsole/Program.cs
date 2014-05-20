namespace SFA.Apprenticeships.Common.Messaging.IntegrationConsole
{
    using System;
    using System.Reflection;
    using SFA.Apprenticeships.Common.Messaging.IntegrationTests.Consumers;

    class Program
    {
        static void Main(string[] args)
        {
            IoC.IoC.Initialize();
            var bus = Transport.CreateBus();
            var bs = new Bootstrapper(bus);
            bs.LoadConsumers(Assembly.GetAssembly(typeof(TestMessage)), "test_app");

            Console.WriteLine("Enter 'q' to quite and any antthing else to send a test message");
            Console.WriteLine("---------------------------------------------------------------");

            string input = Console.ReadLine();

            while (input != "q")
            {
                var testMessage = new TestMessage() {TestString = input};
                bus.Publish(testMessage);
                input = Console.ReadLine();
            }
        }
    }
}
