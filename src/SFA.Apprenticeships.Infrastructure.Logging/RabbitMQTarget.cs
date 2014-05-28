namespace SFA.Apprenticeships.Infrastructure.Logging
{
    using System;
    using System.Configuration;
    using System.Text;
    using EasyNetQ;
    using EasyNetQ.Topology;
    using NLog;
    using NLog.Common;
    using NLog.Targets;
    using SFA.Apprenticeships.Common.Logging.Layouts;
    using SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration;

    /// <summary>
	/// A RabbitMQ-target for NLog that must use a JsonLayout!
	/// </summary>
	[Target("RabbitMQTarget")]
    public class RabbitMQTarget : TargetWithLayout
    {
        private string _rabbitHost;
        private string _queueName = "NLog";
        private string _exchangeName = "app-logging";
        private string _exchangeType = EasyNetQ.Topology.ExchangeType.Topic;
        private string _routingKeyConst = "{0}";
        private string _appId = "SFA.Apprenticeships.App";

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static IRabbitMqHostConfiguration _rabbitMqHostHostConfig;
        private IAdvancedBus _bus;
        private IExchange _exchange;

        #region Target Configuration

        public string RabbitHost
        {
            get { return _rabbitHost; }
            set
            {
                _rabbitMqHostHostConfig = RabbitMqHostsConfiguration.Instance.RabbitHosts[value];
                _rabbitHost = value;
            }
        }

        public string QueueName
        {
            get { return _queueName; }
            set { _queueName = value; }
        }

        public string ExchangeName
        {
            get { return _exchangeName; }
            set { _exchangeName = value; }
        }

        public string ExchangeType
        {
            get { return _exchangeType; }
            set
            {
                switch (value)
                {
                    case EasyNetQ.Topology.ExchangeType.Topic:
                    case EasyNetQ.Topology.ExchangeType.Fanout:
                    case EasyNetQ.Topology.ExchangeType.Header:
                    case EasyNetQ.Topology.ExchangeType.Direct:
                        _exchangeType = value;
                        break;
                    default:
                        throw new ConfigurationErrorsException("ExchangeType not valid ExchangeType, see EasyNetQ.Topology.ExchangeType for valid values");
                }
            }
        }

        public string RoutingKey
        {
            get { return _routingKeyConst; }
            set { _routingKeyConst = value; }
        }

        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        #endregion

        protected override void Write(LogEventInfo logEvent)
		{
			var message = GetMessage(logEvent);
			var routingKey = GetRoutingKey(logEvent.Level.Name);

            var properties = new MessageProperties
            {
                AppId = AppId,
                ContentEncoding = "utf8",
                ContentType = "application/json",
                Timestamp = GetEpochTimeStamp(logEvent),
                UserId = _rabbitMqHostHostConfig.UserName,
                Type = "Log",
                
            };

            _bus.Publish(_exchange, routingKey, true, false, properties, message);
		}

		private string GetRoutingKey(string routeParam)
		{
            var routingKey = string.Format(RoutingKey, routeParam);
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

            if (_rabbitMqHostHostConfig.OutputEasyNetQLogsToNLogInternal)
		    {
		        var logger = new EasyNetNLogInternalLogger();
                _bus = RabbitHutch.CreateBus(_rabbitMqHostHostConfig.HostName, _rabbitMqHostHostConfig.Port, _rabbitMqHostHostConfig.VirtualHost, _rabbitMqHostHostConfig.UserName, _rabbitMqHostHostConfig.Password, _rabbitMqHostHostConfig.HeartBeatSeconds, reg => reg.Register<IEasyNetQLogger>(log => logger)).Advanced;
		    }
		    else
		    {
                _bus = RabbitHutch.CreateBus(_rabbitMqHostHostConfig.HostName, _rabbitMqHostHostConfig.Port, _rabbitMqHostHostConfig.VirtualHost, _rabbitMqHostHostConfig.UserName, _rabbitMqHostHostConfig.Password, _rabbitMqHostHostConfig.HeartBeatSeconds, reg => { }).Advanced;
		    }

            // This will create the exchange and queue and bind them if they doesn't already exist 
            // Change passive from false to true on both calls if it needs to be pre-declared.
            _exchange = _bus.ExchangeDeclare(ExchangeName, ExchangeType, false, _rabbitMqHostHostConfig.Durable);
            var queue = _bus.QueueDeclare(QueueName, false);
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
