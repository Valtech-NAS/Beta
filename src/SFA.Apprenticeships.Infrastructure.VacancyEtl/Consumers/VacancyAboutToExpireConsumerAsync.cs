namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using EasyNetQ.AutoSubscribe;
    using Application.VacancyEtl.Entities;

    public class VacancyAboutToExpireConsumerAsync : IConsumeAsync<VacancyAboutToExpire>
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IExpiringDraftRepository _expiringDraftRepository;
        private readonly IMapper _mapper;

        public VacancyAboutToExpireConsumerAsync(IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository, IExpiringDraftRepository expiringDraftRepository, IMapper mapper)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _expiringDraftRepository = expiringDraftRepository;
            _mapper = mapper;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "VacancyAboutToExpireConsumerAsync")]
        public Task Consume(VacancyAboutToExpire vacancy)
        {
            return Task.Run(() =>
            {
                //Get draft applications for expiring vacancy
                var expiringApplications =
                    _apprenticeshipApplicationReadRepository.GetApplicationSummaries(vacancy.Id)
                        .Where(v => v.Status == ApplicationStatuses.Draft);

                //Map to expiring draft model
                var expiringDrafts =
                    _mapper.Map<IEnumerable<ApprenticeshipApplicationSummary>, IEnumerable<ExpiringDraft>>(
                        expiringApplications).ToList();

                //Write to repo
                expiringDrafts.ForEach(_expiringDraftRepository.Save);
            });
        }
    }
}