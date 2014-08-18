@ignore
Feature: EpicHappyPath
	In order to test the epic happy path
	As a candidate
	I want to be able to search, view, register, login, save, apply, view, resume and dismiss applications

Scenario: Epic happy path
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | Coventry |
		 | WithInDistance | 40 miles |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose FirstVacancyLink
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the LoginPage page
	When I choose CreateAccountLink
	Then I am on the RegisterCandidatePage page
	When I have created a new email address
	And I enter data
		| Field          | Value         |
		| Firstname      | FirstnameTest |
		| Lastname       | LastnameTest  |
		| Phonenumber    | 07970523193   |
		| EmailAddress   | {EmailToken}  |
		| PostcodeSearch | N7 8LS        |
		| Day            | 01            |
		| Month          | 01            |
		| Year           | 2000          |
		| Password       | ?Password01!  | 
	And I choose HasAcceptedTermsAndConditions
	And I choose FindAddresses
	And I am on AddressDropdown list item matching criteria
		| Field        | Rule   | Value                  |
		| Text         | Equals | Flat A, 6 Furlong Road |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I choose CreateAccountButton
	Then I wait 120 second for the ActivationPage page
	When I get the token for my newly created account
	And I wait to see EmailAddress
	And I enter data
		| Field          | Value             |
		| ActivationCode | {ActivationToken} |
	And I choose ActivateButton
	#We end up on the vacancy search page here - did this not take you to the vacnacy you were viewing when you clicked register?
	Then I am on the VacancyDetailsPage page




