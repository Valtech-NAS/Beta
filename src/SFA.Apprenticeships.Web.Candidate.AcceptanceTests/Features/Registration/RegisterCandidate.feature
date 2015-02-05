@US413
Feature: Register Candidate
	In order to apply for a vacnacy
	As an apprentice
	I want to be able to register for the service

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@SmokeTests
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
	And I wait to see ConfirmPassword
	And I wait to see HasAcceptedTermsAndConditions
	And I choose CreateAccountButton
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 12    |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                   |
		| Text  | Equals | Please enter first name |
		| Href  | Equals | #firstname              |
	And I am on the RegisterCandidatePage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Please enter last name |
		| Href  | Equals | #lastname              |

@SmokeTests
Scenario: As a candidate on the registration page I want to be able to pick my address from a list returned from the postcode search
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	And I enter data
		 | Field          | Value  |
		 | PostcodeSearch | N7 8LS |
	And I choose FindAddresses
	And I wait 3 seconds
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

@SmokeTests
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
		
Scenario: As a candidate I want to be told quickly that my email/username is not available if I have already registered
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	And I enter data
		| Field        | Value                |
		| EmailAddress | nas.exemplar+acceptancetests@gmail.com |
	And I choose Phonenumber
	Then I am on the RegisterCandidatePage page
	Given I waited for 30 seconds to see EmailAddressAvailableMessage
	When I am on the RegisterCandidatePage page
	Then I see 
		| Field                        | Rule   | Value                                                                                                                           |
		| EmailAddressAvailableMessage | Equals | Your email address has already been activated. Please try signing in again. If you’ve forgotten your password you can reset it. |

Scenario: As a candidate I want to submit my registration details so that I can apply for vacancies 
	Given I navigated to the RegisterCandidatePage page
	And I have created a new email address
	When I am on the RegisterCandidatePage page
	And I enter data
		 | Field          | Value  |
		 | PostcodeSearch | N7 8LS |
	And I choose FindAddresses
	And I wait 3 seconds
	And I am on AddressDropdown list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Flat A, 6 Furlong Road |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I enter data
		| Field           | Value         |
		| Firstname       | FirstnameTest |
		| Lastname        | LastnameTest  |
		| Day             | 01            |
		| Month           | 01            |
		| Year            | 1999          |
		| EmailAddress    | {EmailToken}  |
		| Phonenumber     | 07999999999   |
		| Password        | ?Password01!  |
		| ConfirmPassword | ?Password01!  |
	And I choose HasAcceptedTermsAndConditions
	And I am on the RegisterCandidatePage page
	And I choose CreateAccountButton
	And I wait 120 second for the ActivationPage page
	And I get the token for my newly created account
	And I am on the ActivationPage page
	And I wait to see EmailAddress
	And I am on the ActivationPage page
	And I enter data
		| Field          | Value             |
		| ActivationCode | {ActivationToken} |
	And I am on the ActivationPage page
	And I choose ActivateButton
	And I am on the ApprenticeshipSearchPage page
	When I select the "first" apprenticeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPage page
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page

Scenario: As a candidate I must confirm my password
	Given I navigated to the RegisterCandidatePage page
	And I have created a new email address
	When I am on the RegisterCandidatePage page
	And I enter data
		 | Field          | Value  |
		 | PostcodeSearch | N7 8LS |
	And I choose FindAddresses
	And I wait 3 seconds
	And I am on AddressDropdown list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Flat A, 6 Furlong Road |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I enter data
		| Field           | Value         |
		| Firstname       | FirstnameTest |
		| Lastname        | LastnameTest  |
		| Day             | 01            |
		| Month           | 01            |
		| Year            | 1999          |
		| EmailAddress    | {EmailToken}  |
		| Phonenumber     | 07999999999   |
		| Password        | ?Password01!  |
		| ConfirmPassword | !10drowssaP?  |
	And I choose HasAcceptedTermsAndConditions
	And I am on the RegisterCandidatePage page
	And I choose CreateAccountButton
	Then I am on the RegisterCandidatePage page
	And I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 1     |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                             |
		| Text  | Equals | Sorry, your passwords don’t match |
		| Href  | Equals | #Password                         |

@SmokeTests
Scenario: I cannot enter letters on day, month and year
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	And I enter data
		| Field           | Value         |
		| Day             | aa            |
		| Month           | aa            |
		| Year            | aaaa          |
	And I choose Phonenumber
	Then I am on the RegisterCandidatePage page
	And I see
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 3     |
	And I am on ValidationFieldErrorItems list item matching criteria
		| Field | Rule   | Value                           |
		| Text  | Equals | The field Day must be a number. |
	And I am on the RegisterCandidatePage page
	And I am on ValidationFieldErrorItems list item matching criteria
		| Field | Rule   | Value                           |
		| Text  | Equals | The field Month must be a number. |
		And I am on the RegisterCandidatePage page
	And I am on ValidationFieldErrorItems list item matching criteria
		| Field | Rule   | Value                           |
		| Text  | Equals | The field Year must be a number. |
