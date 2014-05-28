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

        public override PostExceptionAckStrategy HandleConsumerError(ConsumerExecutionContext context, System.Exception exception)
        {
            return context.Info.Redelivered
                       ? base.HandleConsumerError(context, exception)
                       : PostExceptionAckStrategy.ShouldNackWithRequeue;
        }
    }
}