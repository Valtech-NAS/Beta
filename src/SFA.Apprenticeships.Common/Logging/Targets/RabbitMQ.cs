namespace SFA.Apprenticeships.Common.Logging.Targets
{
    using System;
    using System.Configuration;
    using System.Text;
    using EasyNetQ;
    using EasyNetQ.Topology;
    using NLog;
    using NLog.Common;
    using NLog.Targets;
    using SFA.Apprenticeships.Common.Configuration.Messaging;
    using SFA.Apprenticeships.Common.Logging.Layouts;

    /// <summary>
	/// A RabbitMQ-target for NLog that must use a JsonLayout!
	/// </summary>
	[Target("RabbitMQ")]
    public class RabbitMQ : TargetWithLayout
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static readonly RabbitMQConfigurationSection RabbitMQConfig = RabbitMQConfigurationSection.Instance;
        private IAdvancedBus _bus;
        private IExchange _exchange;

        protected override void Write(LogEventInfo logEvent)
		{
			var message = GetMessage(logEvent);
			var routingKey = GetRoutingKey(logEvent.Level.Name);

            var properties = new MessageProperties
            {
                AppId = RabbitMQConfig.AppId,
                ContentEncoding = "utf8",
                ContentType = "application/json",
                Timestamp = GetEpochTimeStamp(logEvent),
                UserId = RabbitMQConfig.UserName,
                Type = "Log",
                
            };

            _bus.Publish(_exchange, routingKey, true, false, properties, message);
		}

		private string GetRoutingKey(string routeParam)
		{
            var routingKey = string.Format(RabbitMQConfig.RoutingKey, routeParam);
			return routingKey;
		}

        private byte[] GetMessage(LogEventInfo logEvent)
		{
            var jsonLayout = Layout as JsonLayout;
            if (jsonLayout == null)
            {
                throw new ConfigurationErrorsException("The layout configuration must use the JsonLayout");
            }

            string messageJson = Layout.Render(logEvent);

            return Encoding.UTF8.GetBytes(messageJson);
		}

        private static long GetEpochTimeStamp(LogEventInfo @event)
        {
            return Convert.ToInt64((@event.TimeStamp - Epoch).TotalSeconds);
        }

		protected override void InitializeTarget()
		{
			base.InitializeTarget();

            if (RabbitMQConfig.OutputEasyNetQLogsToNLogInternal)
		    {
		        var logger = new EasyNetNLogInternalLogger();
                _bus = RabbitHutch.CreateBus(RabbitMQConfig.HostName, RabbitMQConfig.Port, RabbitMQConfig.VirtualHost, RabbitMQConfig.UserName, RabbitMQConfig.Password, RabbitMQConfig.HeartBeatSeconds, reg => reg.Register<IEasyNetQLogger>(log => logger)).Advanced;
		    }
		    else
		    {
                _bus = RabbitHutch.CreateBus(RabbitMQConfig.HostName, RabbitMQConfig.Port, RabbitMQConfig.VirtualHost, RabbitMQConfig.UserName, RabbitMQConfig.Password, RabbitMQConfig.HeartBeatSeconds, reg => { }).Advanced;
		    }

            _exchange = _bus.ExchangeDeclare(RabbitMQConfig.ExchangeName, ExchangeType.Topic, true, RabbitMQConfig.Durable);
            var queue = _bus.QueueDeclare(RabbitMQConfig.QueueName, true);
            _bus.Bind(_exchange, queue, GetRoutingKey("*"));
		}
		
		/// <summary>
        /// Targets Dispose calls CloseTarget and therefore tidies up any resources open to RabbitMQ
		/// </summary>
		protected override void CloseTarget()
        {
		    base.CloseTarget();
            if (_bus != null)
            {
                _bus.Dispose();
            }
		}

        internal class EasyNetNLogInternalLogger : IEasyNetQLogger
        {
            public void DebugWrite(string format, params object[] args)
            {
                InternalLogger.Debug(format, args);
            }

            public void InfoWrite(string format, params object[] args)
            {
                InternalLogger.Info(format, args);
            }

            public void ErrorWrite(string format, params object[] args)
            {
                InternalLogger.Error(format, args);
            }

            public void ErrorWrite(Exception exception)
            {
                InternalLogger.Error(exception.ToString());
            }
        }
    }
}
