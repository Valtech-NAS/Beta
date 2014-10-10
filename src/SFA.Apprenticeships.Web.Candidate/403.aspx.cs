namespace SFA.Apprenticeships.Web.Candidate
{
    using System;
    using System.Web.UI;
    using Microsoft.WindowsAzure;

    public partial class _403 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var message = CloudConfigurationManager.GetSetting("WebsiteOfflineMessage");

                OfflineMessageLabel.Text = message;
            }
        }
    }
}