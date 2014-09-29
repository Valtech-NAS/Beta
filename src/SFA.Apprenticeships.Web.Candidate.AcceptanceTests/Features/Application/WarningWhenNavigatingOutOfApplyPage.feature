Feature: WarningWhenNavigatingOutOfApplyPage
	As a candidate who is navigating away from the application form
	I want to be warned that I will lose changes if I proceed
	so that I don't lose any changes I have made without consciously deciding to do so

@US501
Scenario: Selecting No I will remain in the same page
	Given I have registered a new candidate
	#When I enter data
	#	| Field    | Value  |
	#	| Location | N7 8LS |
	#And I choose Search
	#Then I am on the VacancySearchResultPage page
	#When I choose FirstVacancyLink
	When I select the first vacancy in location "N7 8LS" that can apply by this website
	Then I am on the VacancyDetailsPage page
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
	Then I am on the ApplicationPage page

@US501
Scenario: Selecting yes will redirect me to another page
	Given I have registered a new candidate
	#When I enter data
	#	| Field    | Value  |
	#	| Location | N7 8LS |
	#And I choose Search
	#Then I am on the VacancySearchResultPage page
	#When I choose FirstVacancyLink
	When I select the first vacancy in location "N7 8LS" that can apply by this website
	Then I am on the VacancyDetailsPage page
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
	Then I am on the MyApplicationsPage page
