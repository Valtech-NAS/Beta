Feature: Vacancy Not found
	As an administrator I want to check that the error pages are configured correctly
	
Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@SmokeTests
Scenario: Vacancy not found should redirect to the error page
	Given I am in the right environment
	When I navigate to the details of the vacancy 199999
	Then I am on the ApprenticeshipDetailsPage page
	And I see
		| Field                                  | Rule   | Value                              |
		| ApprenticeshipNoLongerAvailableHeading | Exists |                                    |
		| ApprenticeshipNoLongerAvailableHeading | Equals | Apprenticeship no longer available |
