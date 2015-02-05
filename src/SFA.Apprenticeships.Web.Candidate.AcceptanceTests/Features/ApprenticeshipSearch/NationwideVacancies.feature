﻿@US500
Feature: Nationwide apprenticeships
	As a candidate
	I want to be able to see apprenticeships that exist nationwide
	so that I can see opportunities that may be of interest to me irrespective of my location

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@SmokeTests
Scenario: After search I see the local apprenticeships
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Does Not Exist |       |
        | NationwideLocationTypeLink | Exists         |       |

@SmokeTests
Scenario: After clicking on nationwide apprenticeships I see them
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                        | Rule           | Value |
        | LocalLocationTypeLink        | Exists         |       |
        | NationwideLocationTypeLink   | Does Not Exist |       |

@SmokeTests
Scenario: Nationwide apprenticeships cannot have their sort order changed
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	Then I wait 0 seconds for SortOrderingDropDown to become disabled

@SmokeTests
Scenario: Nationwide apprenticeships do not show distance
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	Then I see SearchResults list contains
        | Field                | Rule   | Value |
        | DistanceDisplayed    | Equals | False |
        | ClosingDateDisplayed | Equals | True  |
        | NationwideDisplayed  | Equals | True  |

@SmokeTests
Scenario: Nationwide apprenticeships are in closing date order
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | ResultsAreInClosingDateOrder | Equals | True  |

@SmokeTests
Scenario: Nationwide apprenticeships found by keyword are in best match order
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | it         |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | ResultsAreInClosingDateOrder | Equals | False |

@SmokeTests
Scenario: Nationwide apprenticeships found by keyword can be ordered
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | it         |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                      | Rule         | Value        |
        | SearchResultItemsCount     | Greater Than | 0            |
        | SortOrderingDropDown       | Equals       | Best Match   |
        | NationwideLocationTypeLink | Exists       |              |
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                          | Rule           | Value                   |
        | LocalLocationTypeLink          | Exists         |                         |
        | NationwideLocationTypeLink     | Does Not Exist |                         |
        | SortOrderingDropDownItemsCount | Equals         | 2                       |
        | SortOrderingDropDownItemsText  | Equals         | Best Match,Closing Date |
        | SortOrderingDropDown           | Equals         | Best Match              |
        | ResultsAreInClosingDateOrder   | Equals         | False                   |

@SmokeTests
Scenario: When I'm seeing nationwide apprenticeships and I change the results per page I remain there
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	When I enter data
		| Field                  | Value       |
		| ResultsPerPageDropDown | 25 per page |
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |

@SmokeTests
Scenario: When I'm seeing nationwide apprenticeships and I change the sort order I remain there
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value        |
		 | Keywords            | Construction |
		 | Location            | London       |
		 | WithInDistance      | 40 miles     |
		 | ApprenticeshipLevel | All levels   |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	When I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing Date |
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |

@SmokeTests
Scenario: If there are only nationwide apprenticeships do not show any link
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | Stanton    |
		 | WithInDistance      | 2 miles    |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I am on the ApprenticeshipSearchResultPage page
	Then I see
		| Field                      | Rule           | Value |
        | NationwideLocationTypeLink | Does Not Exist |       |
