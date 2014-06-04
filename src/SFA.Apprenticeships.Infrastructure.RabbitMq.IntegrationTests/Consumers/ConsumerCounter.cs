namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Tests.Consumers
{
    public static class ConsumerCounter
    {
        static int _counter = 0;
        static readonly object _lock = new object();

        public static void IncrementCounter()
        {
            lock (_lock)
            {
                _counter++;
            }
        }

        public static int Counter { get { return _counter; } }
    }
}
