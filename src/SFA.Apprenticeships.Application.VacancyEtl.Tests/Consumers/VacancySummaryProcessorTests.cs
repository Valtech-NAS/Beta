namespace SFA.Apprenticeships.Applcation.VacancyEtl.Tests.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Application.VacancyEtl;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Domain.Interfaces.Services.Mapping;

    [TestFixture]
    public class VacancySummaryProcessorTests
    {
        private Mock<IMessageBus> _busMock;
        private Mock<IMessageService<StorageQueueMessage>> _messagingServiceMock;
        private Mock<IVacancyProvider> _vacancyProviderMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _busMock = new Mock<IMessageBus>();
            _messagingServiceMock = new Mock<IMessageService<StorageQueueMessage>>();
            _vacancyProviderMock = new Mock<IVacancyProvider>();
            _mapperMock = new Mock<IMapper>();
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(4, 7)]
        [TestCase(10, 5)]
        public void ShouldQueueCorrectNumberOfVacancyPages(int nationalVacancyPages, int nonNationalVancanyPages)
        {
            _vacancyProviderMock.Setup(x => x.GetVacancyPageCount(VacancyLocationType.National)).Returns(nationalVacancyPages);
            _vacancyProviderMock.Setup(x => x.GetVacancyPageCount(VacancyLocationType.NonNational)).Returns(nonNationalVancanyPages);

            var vacancyConsumer = new VacancySummaryProcessor(_busMock.Object, _vacancyProviderMock.Object, _messagingServiceMock.Object, _mapperMock.Object);
            
            var scheduledMessage = new StorageQueueMessage()
            {
                ClientRequestId = Guid.NewGuid().ToString(),
                MessageId = "456",
                PopReceipt = "789"
            };
            vacancyConsumer.QueueVacancyPages(scheduledMessage);

            Thread.Sleep(50); //Slight delay to ensure parallel foreach has completed before assertions are made

            _messagingServiceMock.Verify(x => x.DeleteMessage(It.Is<string>(mid => mid == scheduledMessage.MessageId), It.Is<string>(pr => pr == scheduledMessage.PopReceipt)), Times.Once);
            _busMock.Verify(x => x.PublishMessage(It.IsAny<VacancySummaryPage>()), Times.Exactly(nationalVacancyPages + nonNationalVancanyPages));
        }

        [TestCase(VacancyLocationType.National, 0)]
        [TestCase(VacancyLocationType.NonNational, 5)]
        [TestCase(VacancyLocationType.Unknown, 10)]
        public void ShouldQueueCorrectNumberOfVacanySummaries(VacancyLocationType vacancyLocationType, int vacanciesReturned)
        {
            var summaryPage = new VacancySummaryPage { VacancyLocation = vacancyLocationType, PageNumber = vacanciesReturned, UpdateReference = Guid.NewGuid() };

            _vacancyProviderMock.Setup(x => x.GetVacancySummary(vacancyLocationType, vacanciesReturned))
                .Returns((VacancyLocationType vlt, int vr) =>
                {
                    var sumaries = new List<VacancySummary>(vr);
                    for (int i = 1; i <= vr; i++)
                    {
                        var summary = new VacancySummary { Id = i };
                        sumaries.Add(summary);
                    }
                    return sumaries;
                });

            _mapperMock.Setup(
                x =>
                    x.Map<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>(
                        It.IsAny<IEnumerable<VacancySummary>>()))
                .Returns((IEnumerable<VacancySummary> vacanies) =>
                {
                    var vacUpdates = new List<VacancySummaryUpdate>(vacanies.Count());
                    vacanies.ToList()
                        .ForEach(
                            v =>
                                vacUpdates.Add(new VacancySummaryUpdate()
                                {
                                    Id = v.Id,
                                    UpdateReference = summaryPage.UpdateReference
                                }));
                    return vacUpdates;
                });

            var vacancyConsumer = new VacancySummaryProcessor(_busMock.Object, _vacancyProviderMock.Object, _messagingServiceMock.Object, _mapperMock.Object);
            
            vacancyConsumer.QueueVacancySummaries(summaryPage);

            _vacancyProviderMock.Verify(x => x.GetVacancySummary(
                                                It.Is<VacancyLocationType>(vlt => vlt == vacancyLocationType),
                                                It.Is<int>(pn => pn == vacanciesReturned)), 
                                                Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>(It.Is<IEnumerable<VacancySummary>>(vc => vc.Count() == vacanciesReturned)), Times.Once);
            _busMock.Verify(x => x.PublishMessage(It.Is<VacancySummaryUpdate>(vsu => vsu.UpdateReference == summaryPage.UpdateReference)), Times.Exactly(vacanciesReturned));
        }
    }
}
