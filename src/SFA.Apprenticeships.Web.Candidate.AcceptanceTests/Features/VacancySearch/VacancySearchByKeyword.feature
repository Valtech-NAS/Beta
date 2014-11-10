Feature: VacancySearchByKeyword
	In order to quickly find a suitable apprenticeship vacancy
	As a candidate
	I want to be able to find and refine vacancies by keyword

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@US449 @SmokeTests
Scenario: When searching by keyword the results are ordered by best match
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value      |
		 | Keywords       | Mechanical |
		 | Location       | Birmingham |
		 | WithInDistance | 40 miles   |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see 
        | Field                          | Rule         | Value                            |
        | SearchResultItemsCount         | Greater Than | 0                                |
        | SortOrderingDropDownItemsCount | Equals       | 3                                |
        | SortOrderingDropDownItemsText  | Equals       | Best Match,Closing Date,Distance |
        | SortOrderingDropDown           | Equals       | Best Match                       |