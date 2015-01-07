namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using ViewModels.Candidate;

    public abstract class ApplicationMediatorBase : SearchMediatorBase
    {
        protected ApplicationMediatorBase(IConfigurationManager configManager, IUserDataProvider userDataProvider) : base(configManager, userDataProvider)
        {
        }

        protected static IEnumerable<WorkExperienceViewModel> RemoveEmptyRowsFromWorkExperience(
            IEnumerable<WorkExperienceViewModel> workExperience)
        {
            if (workExperience == null)
            {
                return new List<WorkExperienceViewModel>();
            }

            return workExperience.Where(vm =>
                vm.Employer != null && !string.IsNullOrWhiteSpace(vm.Employer.Trim()) ||
                vm.JobTitle != null && !string.IsNullOrWhiteSpace(vm.JobTitle.Trim()) ||
                vm.Description != null && !string.IsNullOrWhiteSpace(vm.Description.Trim()) ||
                vm.FromYear != null && !string.IsNullOrWhiteSpace(vm.FromYear.Trim()) ||
                vm.ToYear != null && !string.IsNullOrWhiteSpace(vm.ToYear.Trim())
                ).ToList();
        }

        protected static IEnumerable<QualificationsViewModel> RemoveEmptyRowsFromQualifications(
            IEnumerable<QualificationsViewModel> qualifications)
        {
            if (qualifications == null)
            {
                return new List<QualificationsViewModel>();
            }

            return qualifications.Where(vm =>
                vm.Subject != null && !string.IsNullOrWhiteSpace(vm.Subject.Trim()) ||
                vm.QualificationType != null && !string.IsNullOrWhiteSpace(vm.QualificationType.Trim()) ||
                vm.Grade != null && !string.IsNullOrWhiteSpace(vm.Grade.Trim()) ||
                vm.Year != null && !string.IsNullOrWhiteSpace(vm.Year.Trim())).ToList();
        }
    }
}