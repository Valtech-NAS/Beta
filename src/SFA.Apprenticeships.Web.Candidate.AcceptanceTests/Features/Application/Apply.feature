@US352 @ignore
Feature: Apply for a vacancy
	As a candidate
	I want to submit applications 
	so that it can be reviewed by a Vacancy Manager

Scenario: As a candidate I would like to apply for a vacancy
	Given I registered an account and activated it
	And I navigated to the LoginPage page
	And I entered data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I chose SignInButton
	And I navigated to the VacancySearch page
	And I entered data
		| Field    | Value  |
		| Location | N7 8LS |
	And I chose Search
	And I was on the VacancySearchResultPage page
	#And I set token VacancyId with the value of FirstVacancyId
	And I chose FirstVacancyLink
	And I was on the VacancyDetailsPage page
	And I chose ApplyButton
	And I was on the ApplicationPage page
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


