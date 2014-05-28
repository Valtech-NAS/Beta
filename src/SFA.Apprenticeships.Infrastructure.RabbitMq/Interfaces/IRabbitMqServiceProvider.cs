namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Interfaces
{
    using System;

    internal interface IRabbitMqServiceProvider
    {
        Action<EasyNetQ.IServiceRegister> RegisterCustomServices();
    }
}