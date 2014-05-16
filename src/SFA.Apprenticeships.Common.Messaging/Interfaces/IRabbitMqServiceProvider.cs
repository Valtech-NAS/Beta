namespace SFA.Apprenticeships.Common.Messaging.Interfaces
{
    using System;

    internal interface IRabbitMqServiceProvider
    {
        Action<EasyNetQ.IServiceRegister> RegisterCustomServices();
    }
}