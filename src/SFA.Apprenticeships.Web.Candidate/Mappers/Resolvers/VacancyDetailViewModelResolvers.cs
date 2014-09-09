namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class VacancyDetailViewModelResolvers
    {
        public class EmployerNameResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.IsEmployerAnonymous ?
                    vacancyDetail.AnonymousEmployerName :
                    vacancyDetail.EmployerName;
            }
        }

        public class WageResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.WageType == WageType.Text ?
                    vacancyDetail.WageDescription :
                    string.Format("£{0:N2}", vacancyDetail.Wage);
            }
        }

        // TODO: US490: AG: remove when NAS Gateway service becomes available.
        public class LoremIpsumResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return vacancyDetail.Id % 2 == 0
                    ? null
                    : "Lorem ipsum dolor sit amet, ut vix tota dicunt. Iriure discere imperdiet eam in. Cu molestiae intellegebat sed. Atqui inani et per, eam amet vidit aperiam ad. Pri et affert antiopam interpretaris, vim blandit voluptaria at. Id omnes repudiare eos, deseruisse contentiones duo in.";
            }
        }

        // TODO: US509: AG: remove when NAS Gateway service becomes available.
        public class ApplyViaEmployerWebsiteResolver : ValueResolver<VacancyDetail, bool>
        {
            protected override bool ResolveCore(VacancyDetail vacancyDetail)
            {
                return !string.IsNullOrWhiteSpace(vacancyDetail.Title) &&
                    vacancyDetail.Title.Contains("Bank");
            }
        }

        // TODO: US509: AG: remove when NAS Gateway service becomes available.
        public class VacancyUrlesolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return !string.IsNullOrWhiteSpace(vacancyDetail.Title) && 
                    vacancyDetail.Title.Contains("Bank")
                    ? "http://www.barclays.co.uk"
                    : null;
            }
        }

        // TODO: US509: AG: remove when NAS Gateway service becomes available.
        public class ApplicationInstructionsResolver : ValueResolver<VacancyDetail, string>
        {
            protected override string ResolveCore(VacancyDetail vacancyDetail)
            {
                return !string.IsNullOrWhiteSpace(vacancyDetail.Title) &&
                    vacancyDetail.Title.Contains("Bank")
                    ? "Bacon ipsum dolor sit amet ribeye corned beef pastrami prosciutto. Meatloaf t-bone shankle kielbasa beef ribs, short ribs shank. Salami beef ribs meatball capicola. Ball tip turkey drumstick, ham jowl venison pork belly porchetta landjaeger ham hock. Bacon tail kevin ham hock, pig strip steak pork fatback. Shank jerky fatback tongue ribeye, bacon bresaola beef. Shank shankle ground round kielbasa, doner beef ribs boudin turkey jowl tenderloin porchetta pancetta capicola."
                    : null;
            }
        }
    }
}