@US500
Feature: Nationwide apprenticeships
	As a candidate
	I want to be able to see apprenticeships that exist nationwide
	so that I can see opportunities that may be of interest to me irrespective of my location

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@SmokeTests
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

@SmokeTests
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
        | Field                        | Rule           | Value |
        | LocalLocationTypeLink        | Exists         |       |
        | NationwideLocationTypeLink   | Does Not Exist |       |

@SmokeTests
Scenario: Nationwide apprenticeships cannot have their sort order changed
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | London   |
		 | WithInDistance | 40 miles |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the VacancySearchResultPage page
	Then I wait 0 seconds for SortOrderingDropDown to become disabled

@SmokeTests
Scenario: nationwide apprenticeships are in closing date order
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
        | Field                        | Rule           | Value |
        | ResultsAreInClosingDateOrder | Equals         | True  |

@US500 @SmokeTest
Scenario: Nationwide apprenticeships found by keyword can be ordered
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value       |
		 | Keywords       | bricklaying |
		 | Location       | Birmingham  |
		 | WithInDistance | 40 miles    |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see 
        | Field                      | Rule         | Value      |
        | SearchResultItemsCount     | Greater Than | 0          |
        | SortOrderingDropDown       | Equals       | Best Match |
        | NationwideLocationTypeLink | Exists       |            |
	When I choose NationwideLocationTypeLink
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                          | Rule           | Value                   |
        | LocalLocationTypeLink          | Exists         |                         |
        | NationwideLocationTypeLink     | Does Not Exist |                         |
        | SortOrderingDropDownItemsCount | Equals         | 2                       |
        | SortOrderingDropDownItemsText  | Equals         | Best Match,Closing Date |
        | SortOrderingDropDown           | Equals         | Best Match              |
        | ResultsAreInClosingDateOrder   | Equals         | True                    |

@SmokeTests
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

@SmokeTests
Scenario: When I'm seeing nationwide apprenticeships and I change the sort order I remain there
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value        |
		 | Keywords | Construction |
		 | Location | London       |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the VacancySearchResultPage page
	When I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing Date |
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |
