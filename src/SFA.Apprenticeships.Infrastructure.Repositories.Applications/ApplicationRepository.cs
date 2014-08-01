namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using NLog;

    public class ApplicationRepository : GenericMongoClient<MongoApplicationDetail>, IApplicationReadRepository,
        IApplicationWriteRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;

        public ApplicationRepository(IConfigurationManager configurationManager, IMapper mapper)
            : base(configurationManager, "Applications.mongoDB", "applications")
        {
            _mapper = mapper;
        }

        public void Delete(Guid id)
        {
            Logger.Debug("Called Mongodb to delete ApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoApplicationDetail>.EQ(o => o.Id, id));
        }

        public ApplicationDetail Save(ApplicationDetail entity)
        {
            Logger.Debug("Called Mongodb to save ApplicationDetail EntityId={0}, Status={1}", entity.EntityId, entity.Status);

            var mongoEntity = _mapper.Map<ApplicationDetail, MongoApplicationDetail>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved ApplicationDetail to Mongodb with Id={0}", entity.EntityId);

            return _mapper.Map<MongoApplicationDetail, ApplicationDetail>(mongoEntity);
        }

        public ApplicationDetail Get(Guid id)
        {
            Logger.Debug("Called Mongodb to get ApplicationDetail EntityId={0}", id);

            // TODO: US352: STUBIMPL
            return new ApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                Status = ApplicationStatuses.Submitting,
                CandidateId = Guid.NewGuid(),
                LegacyApplicationId = 12345,
                Vacancy = new VacancySummary
                {
                    Id = 12345 // legacy vacancy id  
                },
                CandidateDetails =
                {
                    FirstName = "Johnny",
                    LastName = "Candidate",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    EmailAddress = "email@server.com",
                    PhoneNumber = "07777111222",
                    Address =
                    {
                        AddressLine1 = "Address line 1",
                        AddressLine2 = "Address line 2",
                        AddressLine3 = "Address line 3",
                        AddressLine4 = "Address line 4",
                        Postcode = "CV1 2WT"
                    }
                },
                CandidateInformation =
                {
                    EducationHistory = new Education
                    {
                        Institution  = "Bash Street School",
                        FromYear = 2009,
                        ToYear = 2012
                    },
                    AboutYou =
                    {
                        HobbiesAndInterests = "Hobbies and interests",
                        Improvements = "Improvements are not needed",
                        Strengths = "My strengths are many",
                        Support = "Third line"
                    }
                }
            };
        }

        public ApplicationDetail Get(Expression<Func<ApplicationDetail, bool>> filter)
        {
            //todo: return the first application that matches the filter
            // .FirstOrDefault()
            throw new NotImplementedException();
        }

        public IList<ApplicationSummary> GetForCandidate(Guid candidateId)
        {
            //todo: retrieve applications for the specified candidate, should exclude any that are archived
            throw new NotImplementedException();
        }

        public ApplicationDetail GetForCandidate(Guid candidateId, Func<ApplicationDetail, bool> filter)
        {
            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            var applicationDetailsList = _mapper.Map<MongoApplicationDetail[], IEnumerable<ApplicationDetail>>(
                mongoApplicationDetailsList);

            return applicationDetailsList
                .Where(filter)
                .SingleOrDefault(); // we expect zero or 1
        }
    }
}
