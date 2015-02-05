@US625
Feature: BrowseBasedSearch
	In order to quickly find a suitable apprenticeship vacancy
	As a candidate
	I want to be able to browse by category and sub category to refine my search

@SmokeTests
Scenario: Browse based search tab switch
	Given I navigated to the ApprenticeshipSearchPage page
	When I choose CategoriesTab
	Then I am on the ApprenticeshipSearchPage page
	Then I see
        | Field              | Rule           | Value |
        | Keywords           | Does Not Exist |       |
        | Categories         | Exists         |       |
        | CategoryItemsCount | Greater Than   | 0     |
	Then I am on the ApprenticeshipSearchPage page
	When I choose KeywordsTab
	And I am on the ApprenticeshipSearchPage page
	Then I see
        | Field      | Rule           | Value |
        | Keywords   | Exists         |       |
        | Categories | Does not Exist |       |

@SmokeTests
Scenario: Browse based search happy path
	Given I navigated to the ApprenticeshipSearchPage page
	When I choose CategoriesTab
	Then I see
        | Field              | Rule         | Value |
        | Categories         | Exists       |       |
        | CategoryItemsCount | Greater Than | 0     |
	When I am on CategoryItems list item matching criteria
		| Field | Rule   | Value                            |
		| Text  | Equals | Business, Administration and Law |
	And I choose CategoryRadioButton
	And I am on the ApprenticeshipSearchPage page
	And I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Browse	
	Then I am on the ApprenticeshipSearchResultPage page
	Then I see
        | Field                  | Rule         | Value |
        | SearchResultItemsCount | Equals       | 5     |
        | BrowseTab              | Exists       |       |
        | Categories             | Exists       |       |
        | CategoryItemsCount     | Greater Than | 0     |

@SmokeTests
Scenario: Switch to browse based search
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
        | Field    | Rule   | Value      |
        | Keywords | Equals | Mechanical |
	When I choose BrowseTab
	Then I see
        | Field              | Rule         | Value |
        | Categories         | Exists       |       |
        | CategoryItemsCount | Greater Than | 0     |
	And I am on the ApprenticeshipSearchResultPage page
	When I am on CategoryItems list item matching criteria
		| Field         | Rule   | Value                                     |
		| CategoryLabel | Equals | Agriculture, Horticulture and Animal Care |
	And I choose WrappedElement
	And I am on the ApprenticeshipSearchResultPage page
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	Then I see
        | Field              | Rule         | Value |
        | BrowseTab          | Exists       |       |
        | Categories         | Exists       |       |
        | CategoryItemsCount | Greater Than | 0     |
	When I choose SearchTab
	Then I see
        | Field     | Rule   | Value      |
        | SearchTab | Exists |            |
        | Keywords  | Equals | Mechanical |
