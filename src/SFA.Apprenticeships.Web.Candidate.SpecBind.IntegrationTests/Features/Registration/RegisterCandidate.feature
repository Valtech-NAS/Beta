@US413
Feature: Register Candidate
	In order to apply for a vacnacy
	As an apprentice
	I want to be able to register for the service

Scenario: As a candidate I am on the registration page and all required fields are present and all validators show
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	And I wait to see Firstname
	And I wait to see Lastname
	And I wait to see Day
	And I wait to see Month
	And I wait to see Year
	And I wait to see EmailAddress
	And I wait to see Password
	And I wait to see HasAcceptedTermsAndConditions
	And I choose CreateAccountButton
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 9     |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                         |
		| Text  | Equals | 'First name' must be supplied |
		| Href  | Equals | #firstname                    |
	And I am on the RegisterCandidatePage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                       |
		| Text  | Equals | 'Last name' must be supplied |
		| Href  | Equals | #lastname                   |

Scenario: As a candidate on the registration page I want to be able to pick my address from a list returned from the postcode search
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	And I enter data
		 | Field          | Value  |
		 | PostcodeSearch | N7 8LS |
	And I choose FindAddresses
	And I am on AddressDropdown list item matching criteria
		| Field        | Rule   | Value                  |
		| Text         | Equals | Flat A, 6 Furlong Road |
		| AddressLine1 | Equals | Flat A                 |
		| AddressLine2 | Equals | 6 Furlong Road         |
		| AddressLine3 | Equals | London                 |
		| AddressLine4 | Equals | Islington              |
		| Postcode     | Equals | N7 8LS                 |
		| Uprn         | Equals | 5300034721             |
		| Latitude     | Equals | 51.54751633697479      |
		| Longitude    | Equals | -0.10660693737952387   |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	Then I see
		| Field        | Rule   | Value                |
		| AddressLine1 | Equals | Flat A               |
		| AddressLine2 | Equals | 6 Furlong Road       |
		| AddressLine3 | Equals | London               |
		| AddressLine4 | Equals | Islington            |
		| Postcode     | Equals | N7 8LS               |
		| Uprn         | Equals | 5300034721           |
		| Latitude     | Equals | 51.54751633697479    |
		| Longitude    | Equals | -0.10660693737952387 |


Scenario: As a candidate I want to be told quickly that my email/username is available
	Given I navigated to the RegisterCandidatePage page
	And I have created a new email address
	When I am on the RegisterCandidatePage page
	And I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose Phonenumber
	And I am on the RegisterCandidatePage page
	Then I see 
		| Field                        | Rule           | Value |
		| EmailAddressAvailableMessage | Does Not Exist |       |
		
@ignore
Scenario: As a candidate I want to be told quickly that my email/username is not available if I have already registered
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	And I enter data
		| Field        | Value                |
		| EmailAddress | valtechnas@gmail.com |
	And I choose Phonenumber
	And I am on the RegisterCandidatePage page
	And I wait to see EmailAddressAvailableMessage
	And I am on the RegisterCandidatePage page
	Then I see 
		| Field                        | Rule   | Value                              |
		| EmailAddressAvailableMessage | Equals | Username already in use, try again |

Scenario: As a candidate I want to be submit my registration details so that I can apply for vacancies 
	Given I navigated to the RegisterCandidatePage page
	And I have created a new email address
	When I am on the RegisterCandidatePage page
	And I enter data
		 | Field          | Value  |
		 | PostcodeSearch | N7 8LS |
	And I choose FindAddresses
	And I am on AddressDropdown list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Flat A, 6 Furlong Road |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I enter data
		| Field        | Value         |
		| Firstname    | FirstnameTest |
		| Lastname     | LastnameTest  |
		| Day          | 01            |
		| Month        | 01            |
		| Year         | 1999          |
		| EmailAddress | {EmailToken}  |
		| Phonenumber  | 07999999999   |
		| Password     | ?Password01!  |
	And I choose HasAcceptedTermsAndConditions
	And I am on the RegisterCandidatePage page
	And I choose CreateAccountButton
	And I wait 90 second for the ActivationPage page
	And I get the token for my newly created account
	And I am on the ActivationPage page
	And I wait to see EmailAddress
	And I am on the ActivationPage page
	And I enter data
		| Field          | Value             |
		| ActivationCode | {ActivationToken} |
	And I am on the ActivationPage page
	And I choose ActivateButton
	And I am on the VacancySearchPage page
	And I enter data
		| Field    | Value  |
		| Location | N7 8LS |
	And I choose Search
	And I am on the VacancySearchResultPage page	
	And I choose FirstVacancyLink
	And I am on the VacancyDetailsPage page
	And I choose Apply
	Then I am on the ApplicationPage page