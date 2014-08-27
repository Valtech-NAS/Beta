@ignore
Feature: EpicHappyPath
	In order to test the epic happy path
	As a candidate
	I want to be able to search, view, register, login, save, apply, view, resume and dismiss applications

#TODO Refine background step to support cookie detection
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

Scenario: Epic happy path
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | Coventry |
		 | WithInDistance | 40 miles |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose FirstVacancyLink
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the LoginPage page
	When I choose CreateAccountLink
	Then I am on the RegisterCandidatePage page
	When I have created a new email address
	And I enter data
		| Field          | Value         |
		| Firstname      | FirstnameTest |
		| Lastname       | LastnameTest  |
		| Phonenumber    | 07970523193   |
		| EmailAddress   | {EmailToken}  |
		| PostcodeSearch | N7 8LS        |
		| Day            | 01            |
		| Month          | 01            |
		| Year           | 2000          |
		| Password       | ?Password01!  | 
	And I choose HasAcceptedTermsAndConditions
	And I choose FindAddresses
	And I am on AddressDropdown list item matching criteria
		| Field        | Rule   | Value                  |
		| Text         | Equals | Flat A, 6 Furlong Road |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I choose CreateAccountButton
	Then I wait 120 second for the ActivationPage page
	When I get the token for my newly created account
	And I enter data
		| Field          | Value             |
		| ActivationCode | {ActivationToken} |
	And I choose ActivateButton
	Then I am on the ApplicationPage page
	When I choose SupportMeYes
	When I enter data
		| Field                   | Value                         |
		| EducationNameOfSchool   | SchoolName                    |
		| EducationFromYear       | 2010                          |
		| EducationToYear         | 2012                          |
		| WhatAreYourStrengths    | My strengths                  |
		| WhatCanYouImprove       | What can I improve            |
		| HobbiesAndInterests     | Hobbies and interests         |
		| WhatCanWeDoToSupportYou | What can we do to support you |
	And I enter employer question data if present
		| Field                                              | Value |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer1 | Emp 1 |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer2 | Emp 2 |
	And I choose ApplyButton
	Then I am on the ApplicationPreviewPage page
	And I see
		| Field                   | Rule   | Value                         |
		| Fullname                | Equals | Firstname Lastname            |
		| Phonenumber             | Equals | 07970523193                   |
		| EmailAddress            | Equals | {EmailToken}                  |
		| Postcode                | Equals | N7 8LS                        |
		| DateOfBirth             | Equals | 01 January 2000               |
		| EducationNameOfSchool   | Equals | SchoolName                    |
		| EducationFromYear       | Equals | 2010                          |
		| EducationToYear         | Equals | 2012                          |
		| WhatAreYourStrengths    | Equals | My strengths                  |
		| WhatCanYouImprove       | Equals | What can I improve            |
		| HobbiesAndInterests     | Equals | Hobbies and interests         |
		| WhatCanWeDoToSupportYou | Equals | What can we do to support you |
	When I choose SubmitApplication
	Then I am on the ApplicationCompletePage page
