Feature: Apprenticeship Application Pre Populate
	In order to speed up the application process
	As a candidate
	I want valid data I have previously entered to pre populate the application form

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@US461
Scenario: Pre-populate my personal and contact details
	Given I have registered a new candidate
	When I select the "first" vacancy in location "N7 8LS" that can apply by this website
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	And I see
		| Field                   | Rule   | Value              |
		#Should have been filled in automatically from personal details
		| FullnameReadOnly        | Equals | Firstname Lastname |
		| EmailReadOnly           | Equals | {EmailToken}       |
		| DobReadOnly             | Equals | 01/01/2000         |
		| PhoneReadOnly           | Equals | 07970523193        |
		| AddressLine1ReadOnly    | Equals | Flat A             |
		| AddressLine2ReadOnly    | Equals | 6 Furlong Road     |
		| AddressLine3ReadOnly    | Equals | London             |
		| AddressLine4ReadOnly    | Equals | Islington          |
		| AddressPostcodeReadOnly | Equals | N7 8LS             |
		#Should not be filled in as no previous application has been submitted
		| EducationNameOfSchool   | Equals |                    |
		| EducationFromYear       | Equals |                    |
		| EducationToYear         | Equals |                    |
		| WhatAreYourStrengths    | Equals |                    |
		| WhatCanYouImprove       | Equals |                    |
		| HobbiesAndInterests     | Equals |                    |

@US461
Scenario: Pre-populate my Education Qualifications Work Experience About You details
	Given I have registered a new candidate
	When I select the "first" vacancy in location "N7 8LS" that can apply by this website
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
	And I choose SaveButton
	Then I wait to see ApplicationSavedMessage
	And I see
		| Field                   | Rule      | Value           |
		| ApplicationSavedMessage | Ends With | my applications |
	When I select the "second" vacancy in location "N7 8LS" that can apply by this website
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	And I see
		| Field                 | Rule   | Value                 |
		| EducationNameOfSchool | Equals | SchoolName            |
		| EducationFromYear     | Equals | 2010                  |
		| EducationToYear       | Equals | 2012                  |
		| WhatAreYourStrengths  | Equals | My strengths          |
		| WhatCanYouImprove     | Equals | What can I improve    |
		| HobbiesAndInterests   | Equals | Hobbies and interests |