Feature: ApprenticeshipSearchByReferenceNumber
	In order to quickly find a specific apprenticeship vacancy
	As a candidate
	I want to be able to find apprenticeship vacancies by their reference number

@US659 @SmokeTests
Scenario: When searching by reference number and finding no result I am advised to use a different number
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field    | Value        |
		 | Keywords | VAC999999999 |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                 | Rule   | Value |
        | ReferenceNumberAdvice | Exists |       |

@US659 @SmokeTests
Scenario: When searching by reference number and finding a result I am taken to the details page
	Given I select the "first" apprenticeship vacancy in location "London"
	Then I am on the ApprenticeshipDetailsPage page
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field    | Value              |
		 | Keywords | {VacancyReference} |
	And I choose Search
	Then I am on the ApprenticeshipDetailsPage page