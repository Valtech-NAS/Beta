namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Applications;

    public class SettingsViewModelBuilder
    {
        private bool _allowEmailComms;
        private bool _allowSmsComms;
        private bool _showTraineeshipsLink;
        private bool _showTraineeshipsPrompt;

        public SettingsViewModelBuilder AllowEmailComms(bool allowEmailComms)
        {
            _allowEmailComms = allowEmailComms;
            return this;
        }

        public SettingsViewModelBuilder AllowSmsComms(bool allowSmsComms)
        {
            _allowSmsComms = allowSmsComms;
            return this;
        }

        public SettingsViewModelBuilder ShowTraineeshipsLink(bool showTraineeshipsLink)
        {
            _showTraineeshipsLink = showTraineeshipsLink;
            return this;
        }

        public SettingsViewModelBuilder ShowTraineeshipsPrompt(bool showTraineeshipsPrompt)
        {
            _showTraineeshipsPrompt = showTraineeshipsPrompt;
            return this;
        }

        public SettingsViewModel Build()
        {
            var model = new SettingsViewModel
            {
                AllowEmailComms = _allowEmailComms,
                AllowSmsComms = _allowSmsComms,
                TraineeshipFeature = new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsLink = _showTraineeshipsLink,
                    ShowTraineeshipsPrompt = _showTraineeshipsPrompt
                }
            };

            return model;
        }
    }
}