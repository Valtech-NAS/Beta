namespace SFA.Apprenticeships.Common.Logging.Configuration
{
    public interface IRabbitMQLoggingConfiguration
    {
        /// <summary>
        /// 	Gets or sets the virtual host to publish to.
        /// </summary>
        string VirtualHost { get; set; }


        /// <summary>
        /// 	Gets or sets the username to use for
        /// 	authentication with the message broker. The default
        /// 	is 'guest'
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 	Gets or sets the password to use for
        /// 	authentication with the message broker.
        /// 	The default is 'guest'
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// 	Gets or sets the port to use
        /// 	for connections to the message broker (this is the broker's
        /// 	listening port).
        /// 	The default is '5672'.
        /// </summary>
        ushort Port { get; set; }

        ///<summary>
        ///	Gets or sets the routing key (aka. topic) with which
        ///	to send messages. Defaults to {0}, which in the end is 'error' for log.Error("..."), and
        ///	so on. An example could be setting this property to 'ApplicationType.MyApp.Web.{0}'.
        ///	The default is '{0}'.
        ///</summary>
        string RoutingKey { get; set; }

        /// <summary>
        /// 	Gets or sets the host name of the broker to log to.
        /// </summary>
        /// <remarks>
        /// 	Default is 'localhost'
        /// </remarks>
        string HostName { get; set; }

        /// <summary>
        /// 	Gets or sets the exchange to bind the logger output to.
        /// </summary>
        /// <remarks>
        /// 	Default is 'log4net-logging'
        /// </remarks>
        string ExchangeName { get; set; }

        /// <summary>
        ///   Gets or sets the exchange type to bind the logger output to.
        /// </summary>
        /// <remarks>
        ///   Default is 'topic'
        /// </remarks>
        string ExchangeType { get; set; }

        /// <summary>
        /// 	Gets or sets the setting specifying whether the exchange
        ///		is durable (persisted across restarts)
        /// </summary>
        /// <remarks>
        /// 	Default is true
        /// </remarks>
        bool Durable { get; set; }

        /// <summary>
        /// 	Gets or sets the application id to specify when sending. Defaults to null,
        /// 	and then IBasicProperties.AppId will be the name of the logger instead.
        /// </summary>
        string AppId { get; set; }

        /// <summary>
        /// Gets or sets the number of heartbeat seconds to have for the RabbitMQ connection.
        /// If the heartbeat times out, then the connection is closed (logically) and then
        /// re-opened the next time a log message comes along.
        /// </summary>
        ushort HeartBeatSeconds { get; set; }

        /// <summary>
        /// The name of the queue to send log entries to.
        /// </summary>
        string QueueName { get; set; }

        /// <summary>
        /// Switches on/off EasyNetQ's logging to NLog's internal logger.
        /// </summary>
        bool OutputEasyNetQLogsToNLogInternal { get; set; }
    }
}
