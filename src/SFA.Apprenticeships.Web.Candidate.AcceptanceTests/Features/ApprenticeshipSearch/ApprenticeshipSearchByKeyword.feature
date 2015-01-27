Feature: ApprenticeshipSearchByKeyword
	In order to quickly find a suitable apprenticeship vacancy
	As a candidate
	I want to be able to find and refine apprenticeship vacancies by keyword

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@US449 @SmokeTests
Scenario: When searching by keyword the keyword is shown
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | Mechanical |
		 | Location            | Birmingham |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                          | Rule         | Value                            |
        | Keywords                       | Equals       | Mechanical                       |

@US449 @SmokeTests
Scenario: When searching by keyword the results are ordered by best match
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | Mechanical |
		 | Location            | Birmingham |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                          | Rule         | Value                            |
        | SearchResultItemsCount         | Greater Than | 0                                |
        | SortOrderingDropDownItemsCount | Equals       | 3                                |
        | SortOrderingDropDownItemsText  | Equals       | Best Match,Closing Date,Distance |
        | SortOrderingDropDown           | Equals       | Best Match                       |

@US449 @SmokeTests
Scenario: When searching by keywords the results are ordered by best match
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value                  |
		 | Keywords            | Mechanical Engineering |
		 | Location            | Birmingham             |
		 | WithInDistance      | 40 miles               |
		 | ApprenticeshipLevel | All levels             |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                          | Rule         | Value                            |
        | SearchResultItemsCount         | Greater Than | 0                                |
        | SortOrderingDropDownItemsCount | Equals       | 3                                |
        | SortOrderingDropDownItemsText  | Equals       | Best Match,Closing Date,Distance |
        | SortOrderingDropDown           | Equals       | Best Match                       |

@US449 @SmokeTests
Scenario: When searching by keyword then removing keyword and searching again the results are ordered by distance
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | Mechanical |
		 | Location            | Birmingham |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                  | Rule         | Value      |
        | SearchResultItemsCount | Greater Than | 0          |
        | SortOrderingDropDown   | Equals       | Best Match |
	When I clear the Keywords field
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	Then I see
        | Field                          | Rule         | Value                 |
        | SearchResultItemsCount         | Greater Than | 0                     |
        | SortOrderingDropDownItemsCount | Equals       | 2                     |
        | SortOrderingDropDownItemsText  | Equals       | Closing Date,Distance |
        | SortOrderingDropDown           | Equals       | Distance              |

@US449 @SmokeTests
Scenario: When searching by keyword then changing keyword and searching again the new keyword is used
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | Mechanical |
		 | Location            | Birmingham |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                  | Rule         | Value      |
        | SearchResultItemsCount | Greater Than | 0          |
        | Keywords               | Equals       | Mechanical |
	When I clear the Keywords field
	And I enter data
		 | Field    | Value       |
		 | Keywords | bricklaying |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	Then I see
        | Field                          | Rule         | Value                 |
        | SearchResultItemsCount         | Greater Than | 0                     |
        | Keywords                       | Equals       | bricklaying           |

@US449 @SmokeTests
Scenario: When searching by location then adding keyword and searching again the results are ordered by best match
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | Birmingham |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                  | Rule         | Value    |
        | SearchResultItemsCount | Greater Than | 0        |
        | SortOrderingDropDown   | Equals       | Distance |	
	When I enter data
		 | Field          | Value      |
		 | Keywords       | Mechanical |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	Then I see
        | Field                          | Rule         | Value                            |
        | SearchResultItemsCount         | Greater Than | 0                                |
        | SortOrderingDropDownItemsCount | Equals       | 3                                |
        | SortOrderingDropDownItemsText  | Equals       | Best Match,Closing Date,Distance |
        | SortOrderingDropDown           | Equals       | Best Match                       |

#Inclusion of nationwide vacancies in results superceeded by US500
@US449 @SmokeTests
Scenario: Nationwide apprenticeships are included in keyword search results
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
        | Field                      | Rule         | Value      |
        | SearchResultItemsCount     | Greater Than | 0          |
        | SortOrderingDropDown       | Equals       | Best Match |
        | NationwideLocationTypeLink | Exists       |            |

@SmokeTests
Scenario: Find apprenticeships by keyword and change ordering to distance
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | Admin      |
		 | Location            | Coventry   |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see
        | Field                           | Rule   | Value |
        | SearchResultItemsCount          | Equals | 5     |
        #| ResultsAreInBestMatchScoreOrder | Equals | True  |
        #| ResultsAreInDistanceOrder       | Equals | False |
        #| ResultsAreInClosingDateOrder    | Equals | False |
	And I enter data
		| Field                | Value    |
		| SortOrderingDropDown | Distance |
	And I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                     | Rule   | Value |
        | SearchResultItemsCount    | Equals | 5     |
        | ResultsAreInDistanceOrder | Equals | True  |
        #| ResultsAreInClosingDateOrder | Equals | True  |
        #| ResultsAreInBestMatchScoreOrder | Equals | False |

@SmokeTests
Scenario: Find apprenticeships by keyword and change ordering to closing date
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | Admin      |
		 | Location            | Coventry   |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see
        | Field                           | Rule   | Value |
        | SearchResultItemsCount          | Equals | 5     |
        #| ResultsAreInBestMatchScoreOrder | Equals | True  |
        #| ResultsAreInDistanceOrder       | Equals | False |
        #| ResultsAreInClosingDateOrder    | Equals | False |
	And I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing Date |
	And I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | SearchResultItemsCount       | Equals | 5     |
        | ResultsAreInClosingDateOrder | Equals | True  |
        #| ResultsAreInDistanceOrder       | Equals | False |
        #| ResultsAreInBestMatchScoreOrder | Equals | False |