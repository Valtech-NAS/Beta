namespace SFA.Apprenticeships.Web.Candidate
{
    using System;
    using Microsoft.WindowsAzure;
    using Views;

    public partial class _403 : ErrorBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var message = CloudConfigurationManager.GetSetting("WebsiteOfflineMessage");

                OfflineMessageLabel.Text = message;
            }

            SetTitle("403");

            var userJourney = GetUserJourney();
            HeaderTitle.InnerText = userJourney == "Apprenticeship" ? "Find an apprenticeship" : "Find a traineeship";
        }
    }
}