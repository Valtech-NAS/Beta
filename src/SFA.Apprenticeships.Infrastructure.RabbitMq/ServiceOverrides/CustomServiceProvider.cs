namespace SFA.Apprenticeships.Infrastructure.RabbitMq.ServiceOverrides
{
    using System;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Serializers;

    public class CustomServiceProvider : IRabbitMqServiceProvider
    {
        public Action<EasyNetQ.IServiceRegister> RegisterCustomServices()
        {
            JsonSettings.Initialize();

            return svc => svc.Register(OverrideConsumerErrorStrategy)
                             .Register(OverrideSerializer);
        }

        private static EasyNetQ.ISerializer OverrideSerializer(EasyNetQ.IServiceProvider provider)
        {
            return new JsonSerializer(provider.Resolve<EasyNetQ.ITypeNameSerializer>());
        }

        private static EasyNetQ.Consumer.IConsumerErrorStrategy OverrideConsumerErrorStrategy(EasyNetQ.IServiceProvider provider)
        {
            return new ConsumerErrorStrategy(
                provider.Resolve<EasyNetQ.IConnectionFactory>(),
                provider.Resolve<EasyNetQ.ISerializer>(),
                provider.Resolve<EasyNetQ.IEasyNetQLogger>(),
                provider.Resolve<EasyNetQ.IConventions>(),
                provider.Resolve<EasyNetQ.ITypeNameSerializer>());
        }
    }
}