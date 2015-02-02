namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Linq;
    using Application.Interfaces.Logging;
    using EasyNetQ;
    using EasyNetQ.Management.Client;
    using RabbitMq.Configuration;

    public class CheckRabbitMessageQueue : IMonitorTask
    {
        private readonly ILogService _logger;
        public CheckRabbitMessageQueue(ILogService logger)
        {
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check rabbit message queues"; }
        }

        public void Run()
        {
            foreach (IRabbitMqHostConfiguration rabbitHost in RabbitMqHostsConfiguration.Instance.RabbitHosts)
            {
                CheckRabbitHealth(rabbitHost);    
            }            
        }

        private void CheckRabbitHealth(IRabbitMqHostConfiguration rabbitConfiguration)
        {
            var rabbitClient = new ManagementClient(rabbitConfiguration.HostName, rabbitConfiguration.UserName, rabbitConfiguration.Password);

            CheckRabbitQueueConnectivity(rabbitConfiguration.ConnectionString);
            CheckRabbitNodeCount(rabbitClient, rabbitConfiguration);
            CheckRabbitQueueWarningLimit(rabbitClient, rabbitConfiguration);
        }

        private void CheckRabbitQueueConnectivity(string connectionString)
        {
            try
            {
                using (var rabbitBus = RabbitHutch.CreateBus(connectionString))
                {
                    rabbitBus.Publish(new MonitorMessage());
                }
            }
            catch (Exception ex)
            {
                var host = connectionString.Split(new[] { ';' }).First();
                LogError(host, ex);
            }
        }

        private void CheckRabbitNodeCount(ManagementClient managementClient, IRabbitMqHostConfiguration rabbitConfiguration)
        {
            try
            {
                var nodes = managementClient.GetNodes();
                int nodeCount = nodes.Count(n => n.Running);
                if (nodeCount != rabbitConfiguration.NodeCount)
                {
                    _logger.Error("Node count in {0} is incorrect. Expecting {1} but was {2}", managementClient.HostUrl,
                        rabbitConfiguration.NodeCount,
                        nodeCount);
                }
            }
            catch (Exception ex)
            {
                LogError(managementClient.HostUrl, ex);
            }
        }

        private void CheckRabbitQueueWarningLimit(ManagementClient managementClient, IRabbitMqHostConfiguration rabbitConfiguration)
        {
            try
            {
                var rabbitQueues = managementClient.GetQueues();

                foreach (var rabbitQueue in rabbitQueues)
                {
                    if (rabbitQueue.Messages > rabbitConfiguration.QueueWarningLimit)
                    {
                        _logger.Warn(
                            "Queue '{0}' on node '{1}' has exceeded the queue item limit threshold of {2} and currrently has {3} messages queued, please check queue is processing as expected",
                            rabbitQueue.Name, rabbitQueue.Node, rabbitConfiguration.QueueWarningLimit, rabbitQueue.Messages);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(managementClient.HostUrl, ex);
            }            
        }

        private void LogError(string host, Exception exception)
        {
            var message = string.Format("Error while connecting to Rabbit queue on {0}", host);
            _logger.Error(message, exception);
        }

        internal class MonitorMessage
        {
            public string Text
            {
                get { return "Monitor test message"; }
            }
        }
    }
}