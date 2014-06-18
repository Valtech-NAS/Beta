param(
    [string]$configuration,
	[string]$sitename
)

Copy-Item "src\SFA.Apprenticeships.Web.$sitename.IntegrationTests\app.$configuration.config" "src\SFA.Apprenticeships.Web.$sitename.IntegrationTests\app.config"