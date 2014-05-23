namespace SFA.Apprenticeships.Common.Configuration.Messaging
{
    public interface IRabbitMqHostConfiguration
    {
        /// <summary>
        /// 	An alias for this host.
        /// </summary>
        string Name { get; set; }

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

        /// <summary>
        /// 	Gets or sets the host name of the broker to log to.
        /// </summary>
        /// <remarks>
        /// 	Default is 'localhost'
        /// </remarks>
        string HostName { get; set; }

        /// <summary>
        /// 	Gets or sets the setting specifying whether the exchange
        ///		is durable (persisted across restarts)
        /// </summary>
        /// <remarks>
        /// 	Default is true
        /// </remarks>
        bool Durable { get; set; }

        /// <summary>
        /// Gets or sets the number of heartbeat seconds to have for the RabbitMQ connection.
        /// If the heartbeat times out, then the connection is closed (logically) and then
        /// re-opened the next time a log message comes along.
        /// </summary>
        ushort HeartBeatSeconds { get; set; }

        /// <summary>
        /// The number of messages to pull of the queue at a time.
        /// </summary>
        ushort PreFetchCount { get; set; }

        /// <summary>
        /// Switches on/off EasyNetQ's logging to NLog's internal logger.
        /// </summary>
        bool OutputEasyNetQLogsToNLogInternal { get; set; }

        /// <summary>
        /// Connection string to connect to RabbitMQ.
        /// </summary>
        string ConnectionString { get; }
    }
}
