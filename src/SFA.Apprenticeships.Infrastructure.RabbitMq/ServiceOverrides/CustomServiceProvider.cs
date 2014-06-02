namespace SFA.Apprenticeships.Infrastructure.RabbitMq.ServiceOverrides
{
    using System;
    using Interfaces;
    using Serializers;
    using EasyNetQ;
    using EasyNetQ.Consumer;

    public class CustomServiceProvider : IRabbitMqServiceProvider
    {
        public Action<IServiceRegister> RegisterCustomServices()
        {
            JsonSettings.Initialize();

            return svc => svc.Register(OverrideConsumerErrorStrategy)
                             .Register(OverrideSerializer);
        }

        private static ISerializer OverrideSerializer(EasyNetQ.IServiceProvider provider)
        {
            return new Serializers.JsonSerializer(provider.Resolve<ITypeNameSerializer>());
        }

        private static IConsumerErrorStrategy OverrideConsumerErrorStrategy(EasyNetQ.IServiceProvider provider)
        {
            return new ConsumerErrorStrategy(
                provider.Resolve<IConnectionFactory>(),
                provider.Resolve<ISerializer>(),
                provider.Resolve<IEasyNetQLogger>(),
                provider.Resolve<IConventions>(),
                provider.Resolve<ITypeNameSerializer>());
        }
    }
}