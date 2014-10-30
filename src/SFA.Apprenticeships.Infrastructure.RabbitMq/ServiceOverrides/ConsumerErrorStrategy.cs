namespace SFA.Apprenticeships.Infrastructure.RabbitMq.ServiceOverrides
{
    using EasyNetQ;
    using EasyNetQ.Consumer;

    internal class ConsumerErrorStrategy : DefaultConsumerErrorStrategy
    {
        public ConsumerErrorStrategy(IConnectionFactory connectionFactory, ISerializer serializer, IEasyNetQLogger logger, IConventions conventions, ITypeNameSerializer typeNameSerializer) 
            : base(connectionFactory, serializer, logger, conventions, typeNameSerializer)
        {
        }

        // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.

        /*
        public override PostExceptionAckStrategy HandleConsumerError(ConsumerExecutionContext context, System.Exception exception)
        {
            return context.Info.Redelivered
                       ? base.HandleConsumerError(context, exception)
                       : PostExceptionAckStrategy.ShouldNackWithRequeue;
        }
        */
    }
}