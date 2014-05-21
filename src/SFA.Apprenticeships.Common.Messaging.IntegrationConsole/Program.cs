﻿using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.Interfaces.Elasticsearch;
using SFA.Apprenticeships.Services.VacancyEtl.Load;
using SFA.Apprenticeships.Services.VacancyEtl.Queue;
using StructureMap;

namespace SFA.Apprenticeships.Common.Messaging.IntegrationConsole
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            long i = 1;
            Common.IoC.IoC.Initialize();
            ElasticsearchLoad<VacancySummary>.Setup(ObjectFactory.GetInstance<IElasticsearchService>());
            var bus = RabbitQueue.Setup();

            Console.WriteLine("Enter 'q' to quite and any antthing else to send a test message");
            Console.WriteLine("---------------------------------------------------------------");

            var input = Console.ReadLine();

            while (input != "q")
            {
                var testMessage = new VacancySummary()
                {
                    UpdateReference = Guid.NewGuid(),
                    Id = i++,
                    Created = DateTime.Now,
                    ClosingDate = DateTime.Today.AddDays(30),
                };

                bus.Publish(testMessage);
                input = Console.ReadLine();
            }
        }
    }
}
