@US352
Feature: Apply for a vacancy
	As a candidate
	I want to submit applications 
	so that it can be reviewed by a Vacancy Manager

#TODO Refine background step to support cookie detection
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@ignore
Scenario: As a candidate I would like to apply for a vacancy
	Given I have registered a new candidate
	When I enter data
		| Field    | Value  |
		| Location | N7 8LS |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	#And I set token VacancyId with the value of FirstVacancyId
	When I choose FirstVacancyLink
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
	And I enter employer question data if present
		| Field                                              | Value |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer1 | Emp 1 |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer2 | Emp 2 |
	And I choose ApplyButton
	Then I am on the ApplicationPreviewPage page

Scenario: As a candidate I want to save my application as a draft an be able to resume or delete it later
	Given I have registered a new candidate
	When I enter data
		| Field    | Value  |
		| Location | N7 8LS |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose FirstVacancyLink
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
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                  | Rule   | Value |
		| DraftApplicationsCount | Equals | 1     |
	When I choose ResumeLink
	Then I am on the ApplicationPage page
	And I see
		| Field                   | Rule      | Value                 |
		| ApplicationSavedMessage | Ends With | my applications       |
		| EducationNameOfSchool   | Equals    | SchoolName            |
		| EducationFromYear       | Equals    | 2010                  |
		| EducationToYear         | Equals    | 2012                  |
		| WhatAreYourStrengths    | Equals    | My strengths          |
		| WhatCanYouImprove       | Equals    | What can I improve    |
		| HobbiesAndInterests     | Equals    | Hobbies and interests |
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page
	When I choose DeleteLink
	Then I wait to see SuccessMessageText
	And I see
	| Field                       | Rule        | Value                                         |
	| SuccessMessageText          | Equals      | Application has been removed from your drafts |
	| EmptyApplicationHistoryText | Starts With | Your application history is currently empty   |

Scenario: As a candidate I want to enter my qualifications
	Given I have registered a new candidate
	When I enter data
		| Field    | Value  |
		| Location | N7 8LS |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose FirstVacancyLink
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	When I choose QualificationsYes
	And I choose SaveQualification
	Then I wait to see QualificationValidationSummary
	And I see
		| Field                          | Rule     | Value                                |
		| QualificationValidationSummary | Contains | Please complete all fields displayed |
	When I am on QualificationTypeDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | GCSE  |
	And I choose WrappedElement
	And I am on the ApplicationPage page
	When I enter data
		| Field        | Value        |
		| SubjectYear  | 2012         |
		| SubjectName  | SubjectName  |
		| SubjectGrade | SubjectGrade |
	And I choose SaveQualification
	Then I wait to see QualificationsSummary
	Then I see
        | Field                      | Rule   | Value |
        | QualificationsSummaryCount | Equals | 1     |
	And I am on QualificationsSummaryItems list item matching criteria
		| Field   | Rule   | Value        |
		| Subject | Equals | SubjectName  |
		| Year    | Equals | 2012         |
		| Grade   | Equals | SubjectGrade |
	When I am on the ApplicationPage page
	And I choose RemoveQualificationLink
#TODO: check there is no qualifications summary
	When I choose WorkExperienceYes
	And I choose SaveWorkExperience
#TODO: check for validation errors
#	Then I see
#		| Field                               | Rule   | Value |
#		| WorkExperienceValidationErrorsCount | Equals | 5     |
	When I enter data
		| Field        | Value        |
		| WorkEmployer | WorkEmployer |
		| WorkTitle    | WorkTitle    |
		| WorkRole     | WorkRole     |
		| WorkFromYear | 2011         |
		| WorkToYear   | 2012         |
	And I choose SaveWorkExperience
	Then I wait to see WorkExperienceSummary
	Then I see
        | Field                | Rule   | Value |
        | WorkExperiencesCount | Equals | 1     |
	And I am on WorkExperienceSummaryItems list item matching criteria
		| Field        | Rule   | Value        |
		| WorkEmployer | Equals | WorkEmployer |
		| WorkTitle    | Equals | WorkTitle    |
		| WorkRole     | Equals | WorkRole     |