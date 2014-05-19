using System;
using RestSharp.Extensions;
using SFA.Apprenticeships.Common.Entities.Attributes.Elasticsearch;
using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Abstract;

namespace SFA.Apprenticeships.Services.WorkerRole.VacancyEtl.Load
{
    /// <summary>
    /// Monitors the queue for new data items and loads them into the es database.
    /// </summary>
    public class ElasticsearchLoad<T> where T : VacancyId
    {
        private readonly IQueue _queue;
        private readonly IElasticSearchService _service;
        private readonly string _command;

        public ElasticsearchLoad(IElasticSearchService service, IQueue queue)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            _service = service;
            _queue = queue;

            // look for the elasticsearch mapping attribute on the class T
            // find the index and name properties to form the es command
            var mapping = typeof (T).GetAttribute<ElasticsearchMappingAttribute>();
            if (mapping == null || string.IsNullOrEmpty(mapping.Name) || string.IsNullOrEmpty(mapping.Index))
            {
                throw new ArgumentException("The generic type must have the ElasticsearchMapping attribute applied with Name and Index properties.");
            }

            _command = string.Format("/{0}/{1}", mapping.Index, mapping.Name);
        }

        public void Execute()
        {
            var msg = _queue.GetMessage();
            if (msg.HasMessage)
            {
                _service.Execute(_command, msg.Id, msg.Json);
            }
        }
    }
}

/*
 *  var queueConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(queueConnectionString);

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            var queueName = CloudConfigurationManager.GetSetting("QueueName");
            var queue = queueClient.GetQueueReference(queueName);
 * 
 *  // Get the next message
                    var retrievedMessage = queue.GetMessage();

                    if (retrievedMessage != null)
                    {
                        var message = retrievedMessage.AsString;

                        repository.SaveAsync(new QueueResponse {Message = message});

                        queue.DeleteMessage(retrievedMessage);

                        SendEmail(message);
                    }
 */