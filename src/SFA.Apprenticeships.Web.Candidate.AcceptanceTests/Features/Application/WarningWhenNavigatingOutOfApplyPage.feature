Feature: WarningWhenNavigatingOutOfApplyPage
	As a candidate who is navigating away from the application form
	I want to be warned that I will lose changes if I proceed
	so that I don't lose any changes I have made without consciously deciding to do so

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@US501
Scenario: Selecting No I will remain in the same page
	Given I have registered a new candidate
	#When I enter data
	#	| Field    | Value  |
	#	| Location | N7 8LS |
	#And I choose Search
	#Then I am on the ApprenticeshipSearchResultPage page
	#When I choose FirstVacancyLink
	When I select the "first" apprenticeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	When I enter data
		| Field                   | Value                         |
		| EducationNameOfSchool   | SchoolName                    |
		| EducationFromYear       | 2010                          |
		| EducationToYear         | 2012                          |
		| WhatAreYourStrengths    | My strengths                  |
		| WhatCanYouImprove       | What can I improve            |
		| HobbiesAndInterests     | Hobbies and interests         |
	And I choose MyApplicationsLink
	When I see an alert box and select No
	When I am on the ApplicationPage page
	Then I am on the ApplicationPage page
	#This is required cleanup for this test. Without it the next test run will fail when reusing the browser
	When I choose MyApplicationsLink
	And I see an alert box and select Yes
	Then I am on the MyApplicationsPage page

@US501
Scenario: Selecting yes will redirect me to another page
	Given I have registered a new candidate
	#When I enter data
	#	| Field    | Value  |
	#	| Location | N7 8LS |
	#And I choose Search
	#Then I am on the ApprenticeshipSearchResultPage page
	#When I choose FirstVacancyLink
	When I select the "first" apprenticeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	When I enter data
		| Field                   | Value                         |
		| EducationNameOfSchool   | SchoolName                    |
		| EducationFromYear       | 2010                          |
		| EducationToYear         | 2012                          |
		| WhatAreYourStrengths    | My strengths                  |
		| WhatCanYouImprove       | What can I improve            |
		| HobbiesAndInterests     | Hobbies and interests         |
	And I choose MyApplicationsLink
	When I see an alert box and select Yes
	When I am on the MyApplicationsPage page
	Then I am on the MyApplicationsPage page
