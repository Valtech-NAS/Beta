@US500
Feature: Nationwide apprenticeships
	As a candidate
	I want to be able to see apprenticeships that exist nationwide
	so that I can see opportunities that may be of interest to me irrespective of my location

#TODO Refine background step to support cookie detection
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page


Scenario: After search I see the local apprenticeships
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | London   |
		 | WithInDistance | 40 miles |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Does Not Exist |       |
        | NationwideLocationTypeLink | Exists         |       |

Scenario: After clicking on nationwide apprenticeships I see them
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | London   |
		 | WithInDistance | 40 miles |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |

Scenario: When I'm seeing nationwide apprenticeships and I change the results per page I remain there
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | London   |
		 | WithInDistance | 40 miles |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the VacancySearchResultPage page
	When I enter data
		| Field                  | Value       |
		| ResultsPerPageDropDown | 25 per page |
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |

Scenario: When I'm seeing nationwide apprenticeships and I change the sort order I remain there
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Keywords       | Chef     |
		 | Location       | London   |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the VacancySearchResultPage page
	When I enter data
		| Field                | Value    |
		| SortOrderingDropDown | Distance |
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |
