namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Communication;
    using Ploeh.AutoFixture;

    public class ExpiringApprenticeshipApplicationDraftsBuilder
    {
        private List<ExpiringApprenticeshipApplicationDraft> _expiringDrafts;

        public ExpiringApprenticeshipApplicationDraftsBuilder()
        {
            _expiringDrafts = new List<ExpiringApprenticeshipApplicationDraft>();
        }

        public ExpiringApprenticeshipApplicationDraftsBuilder WithExpiringDrafts(int noOfDrafts)
        {
            _expiringDrafts = new Fixture().Build<ExpiringApprenticeshipApplicationDraft>()
                .With(ed => ed.ClosingDate, new DateTime(2015, 01, 31))
                .CreateMany(noOfDrafts)
                .OrderBy(p => p.ClosingDate)
                .ToList();

            return this;
        }

        public ExpiringApprenticeshipApplicationDraftsBuilder WithSpecialCharacterExpiringDrafts(int noOfDrafts)
        {
            _expiringDrafts = new Fixture().Build<ExpiringApprenticeshipApplicationDraft>()
                .With(ed => ed.Title, "Tit|e with sp~cial ch@r$ in \"t")
                .With(ed => ed.EmployerName, "\"Emp|ov~r N@m€\"")
                .With(ed => ed.ClosingDate, new DateTime(2015, 01, 31))
                .CreateMany(noOfDrafts)
                .ToList();

            return this;
        }

        public List<ExpiringApprenticeshipApplicationDraft> Build()
        {
            return _expiringDrafts;
        }
    }
}