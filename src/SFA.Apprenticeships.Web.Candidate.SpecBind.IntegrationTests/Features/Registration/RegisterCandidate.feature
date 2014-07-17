@US413
Feature: Register Candidate
	In order to apply for a vacnacy
	As an apprentice
	I want to be able to register for the service

Scenario: As a candidate I am on the registration page and all required fields are present
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	Then I wait to see Firstname
	And I wait to see Lastname
	And I wait to see Day
	And I wait to see Month
	And I wait to see Year
	And I wait to see EmailAddress
	And I wait to see Password
	And I wait to see HasAcceptedTermsAndConditions

Scenario: As a candidate on the registration page I want to be able to pick my address from a list returned from the postcode search
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	And I enter data
		 | Field          | Value  |
		 | PostcodeSearch | N7 8LS |
	And I choose FindAddresses
	And I wait to see AddressDropdown
	And I am on AddressDropdown list item matching criteria
		    | Field        | Rule   | Value  |
		    | AddressLine1 | Equals | Flat A |
			#| DisplayText  | Equals | Flat A, 6 Furlong Road |
		    #| AddressLine2 | Equals | 6 Furlong Road         |
		    #| AddressLine3 | Equals | London                 |
		    #| AddressLine4 | Equals | Isnlington             |
		    #| Postcode     | Equals | N7 8LS                 |
		    #| Nprn         | Exists |                        |
		    #| Latitude     | Exists |                        |
		    #| Longitude    | Exists |                        |
		    