@US352
Feature: Apply for a vacancy
	As a candidate
	I want to submit applications 
	so that it can be reviewed by a Vacancy Manager

#TODO Refine background step to support cookie detection
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@US486 @US458 @US354
Scenario: As a candidate I would like to apply for a vacancy
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
	When I choose SubmitApplication
	Then I am on the ApplicationCompletePage page
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                      | Rule   | Value |
		| SubmittedApplicationsCount | Equals | 1     |

@US461 @US154 @US458 @US464
Scenario: As a candidate I want to save my application as a draft and be able to resume
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

@US461 @US362 @US365 @US154 @US463 @US352 @US354
Scenario: As a candidate I want to enter my qualifications and work experience
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
	Then I see
		| Field                               | Rule   | Value |
		| QualificationsValidationErrorsCount | Equals | 4     |
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
	And I am on the ApplicationPage page
	And I choose SaveQualification
	Then I wait for 30 seconds to see QualificationsSummary
	Then I see
        | Field                      | Rule   | Value |
        | QualificationsSummaryCount | Equals | 1     |
	And I am on QualificationsSummaryItems list item matching criteria
		| Field   | Rule   | Value        |
		| Subject | Equals | SubjectName  |
		| Year    | Equals | 2012         |
		| Grade   | Equals | SubjectGrade |
	When I choose RemoveQualificationLink
	And I am on the ApplicationPage page
	Then I see
        | Field                      | Rule   | Value |
        | QualificationsSummaryCount | Equals | 0     |
	When I choose WorkExperienceYes
	And I choose SaveWorkExperience
	Then I see
		| Field                               | Rule   | Value |
		| WorkExperienceValidationErrorsCount | Equals | 5     |
	When I enter data
		| Field        | Value        |
		| WorkEmployer | WorkEmployer |
		| WorkTitle    | WorkTitle    |
		| WorkRole     | WorkRole     |
		| WorkFromYear | 2011         |
		| WorkToYear   | 2012         |
	And I choose SaveWorkExperience
	Then I wait for 30 seconds to see WorkExperienceSummary
	Then I see
        | Field                | Rule   | Value |
        | WorkExperiencesCount | Equals | 1     |
	And I am on WorkExperienceSummaryItems list item matching criteria
		| Field      | Rule   | Value        |
		| Employer   | Equals | WorkEmployer |
		| JobTitle   | Equals | WorkTitle    |
		| MainDuties | Equals | WorkRole     |
	When I choose RemoveLink
	And I am on the ApplicationPage page
	Then I see
        | Field                | Rule   | Value |
        | WorkExperiencesCount | Equals | 0     |
#Enter data to save
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
	And I choose QualificationsYes
	And I am on QualificationTypeDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | GCSE  |
	And I choose WrappedElement
	And I am on the ApplicationPage page
	And I enter data
		| Field        | Value        |
		| SubjectYear  | 2012         |
		| SubjectName  | SubjectName  |
		| SubjectGrade | SubjectGrade |
	And I choose SaveQualification
	When I choose WorkExperienceYes
	And I enter data
		| Field        | Value        |
		| WorkEmployer | WorkEmployer |
		| WorkTitle    | WorkTitle    |
		| WorkRole     | WorkRole     |
		| WorkFromYear | 2011         |
		| WorkToYear   | 2012         |
	And I choose SaveWorkExperience
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
        | Field                      | Rule   | Value |
        | QualificationsSummaryCount | Equals | 1     |
	And I am on QualificationsSummaryItems list item matching criteria
		| Field   | Rule   | Value        |
		| Subject | Equals | SubjectName  |
		| Year    | Equals | 2012         |
		| Grade   | Equals | SubjectGrade |
	And I am on the ApplicationPage page
	And I see
        | Field                | Rule   | Value |
        | WorkExperiencesCount | Equals | 1     |
	And I am on WorkExperienceSummaryItems list item matching criteria
		| Field      | Rule   | Value        |
		| Employer   | Equals | WorkEmployer |
		| JobTitle   | Equals | WorkTitle    |
		| MainDuties | Equals | WorkRole     |
	When I am on the ApplicationPage page
	And I choose ApplyButton
	Then I am on the ApplicationPreviewPage page
	And I see
		| Field                 | Rule   | Value                 |
		| Fullname              | Equals | Firstname Lastname    |
		| Phonenumber           | Equals | 07970523193           |
		| EmailAddress          | Equals | {EmailToken}          |
		| Postcode              | Equals | N7 8LS                |
		| DateOfBirth           | Equals | 01 January 2000       |
		| EducationNameOfSchool | Equals | SchoolName            |
		| EducationFromYear     | Equals | 2010                  |
		| EducationToYear       | Equals | 2012                  |
		| WhatAreYourStrengths  | Equals | My strengths          |
		| WhatCanYouImprove     | Equals | What can I improve    |
		| HobbiesAndInterests   | Equals | Hobbies and interests |
	When I choose SubmitApplication
	Then I am on the ApplicationCompletePage page
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                      | Rule   | Value |
		| SubmittedApplicationsCount | Equals | 1     |