Feature: Apprenticeship Application Validation
	In order to ensure I do not miss submitting important data
	As a candidate
	I want to be told of any fields I have missed or entered invalid data for

@US461
Scenario: Complete validation path
	Given I have registered a new candidate
	When I select the "first" apprenticeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	And I see
        | Field                      | Rule   | Value |
        | EducationNameOfSchoolError | Exists |       |
        | EducationFromYearError     | Exists |       |
        | EducationToYearError       | Exists |       |