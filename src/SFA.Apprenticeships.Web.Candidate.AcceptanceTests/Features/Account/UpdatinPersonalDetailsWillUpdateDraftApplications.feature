@US515
Feature: Show my up-to-date details
	As a candidate who has updated my personal details
	I want to see the most up to date details on my existing draft applications
	so that when I submit my application it contains my most recent personal details

#TODO Refine background step to support cookie detection
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

Scenario: Updating personal details will update draft applications
	Given I have registered a new candidate
	When I select the first vacancy in location "N7 8LS" that can apply by this website
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	When I choose SupportMeYes
	And I enter data
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
	
	Given I navigated to the SettingsPage page
	When I am on the SettingsPage page
	When I enter data
	| Field        | Value               |
	| Firstname    | Jane                |
	And I choose UpdateDetailsButton
	Then I am on the SettingsPage page

	Given I navigated to the MyApplicationsPage page
	Then I am on the MyApplicationsPage page
	And I see
		| Field                  | Rule   | Value |
		| DraftApplicationsCount | Equals | 1     |
	When I choose ResumeLink
	Then I am on the ApplicationPage page
	And I see
		| Field            | Rule     | Value |
		| FullnameReadOnly | Contains | Jane  |

	When I choose ApplyButton
	Then I am on the ApplicationPreviewPage page
	And I see
		| Field    | Rule     | Value |
		| Fullname | Contains | Jane  |

	When I select the first vacancy in location "London" that can apply by this website
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	And I see
		| Field            | Rule     | Value |
		| FullnameReadOnly | Contains | Jane  |