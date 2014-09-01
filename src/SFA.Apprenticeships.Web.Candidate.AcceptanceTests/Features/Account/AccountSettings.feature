@US483
Feature: Account Settings - Personal Details
	As a candidate 
	I want to be able to make amendments to my first name, last name, date of birth, address and mobile phone number
	so that I can manage my personal details and make sure they are correct

# TODO: Refine background step to support cookie detection.
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

	Given I registered an account and activated it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page

Scenario: As a candidate I can change my personal settings

	Given I navigated to the SettingsPage page
	When I am on the SettingsPage page

	And I wait to see Firstname
	And I wait to see Lastname
	
	And I wait to see Day
	And I wait to see Month
	And I wait to see Year

	And I wait to see PhoneNumber

	And I wait to see PostcodeSearch

	And I wait to see AddressLine1
	And I wait to see AddressLine2
	And I wait to see AddressLine3
	And I wait to see AddressLine4
	And I wait to see Postcode

	And I wait to see UpdateDetailsButton

	Then I see
	| Field            | Rule   | Value |
	| ClearAllSettings | Equals | Done  |

	When I enter data
	| Field        | Value               |
	| Firstname    | Jane                |
	| Lastname     | Dovedale            |
	| Day          | 31                  |
	| Month        | 1                   |
	| Year         | 1994                |
	| Phonenumber  | 07123000099         |
	| AddressLine1 | 10 Downing Street   |
	| AddressLine2 | City of Westminster |
	| AddressLine3 | London              |
	| AddressLine4 | England             |
	| Postcode     | SW1A 2AA            |

	And I choose UpdateDetailsButton
	Then I am on the SettingsPage page

	And I see 
	| Field             | Rule           | Value |
	| ValidationSummary | Does Not Exist |       |

	And I see
	| Field          | Rule   | Value               |
	| Firstname      | Equals | Jane                |
	| Lastname       | Equals | Dovedale            |
	| Day            | Equals | 31                  |
	| Month          | Equals | 1                   |
	| Year           | Equals | 1994                |
	| Phonenumber    | Equals | 07123000099         |
	| AddressLine1   | Equals | 10 Downing Street   |
	| AddressLine2   | Equals | City of Westminster |
	| AddressLine3   | Equals | London              |
	| AddressLine4   | Equals | England             |
	| Postcode       | Equals | SW1A 2AA            |
	| BannerUserName | Equals | Jane Dovedale       |

	And I see
	| Field              | Rule   | Value                  |
	| SuccessMessageText | Equals | TODO: settings updated |

Scenario: As a candidate I cannot save invalid personal settings

	Given I navigated to the SettingsPage page
	When I am on the SettingsPage page
	
	And I wait to see UpdateDetailsButton

	Then I see
	| Field            | Rule   | Value |
	| ClearAllSettings | Equals | Done  |

	When I choose UpdateDetailsButton
	Then I am on the SettingsPage page

	And I see
    | Field                  | Rule   | Value |
    | ValidationSummaryCount | Equals | 8     |

	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                   |
	| Text  | Equals | Please enter first name |
	| Href  | Equals | #FirstName              |

	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                  |
	| Text  | Equals | Please enter last name |
	| Href  | Equals | #LastName              |

	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                |
	| Text  | Equals | Please enter the day |
	| Href  | Equals | #DateOfBirth_Day     |

	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                  |
	| Text  | Equals | Please enter the month |
	| Href  | Equals | #DateOfBirth_Month     |

	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                 |
	| Text  | Equals | Please enter the year |
	| Href  | Equals | #DateOfBirth_Year     |

	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                                   |
	| Text  | Equals | Please enter your first line of address |
	| Href  | Equals | #Address_AddressLine1                   |

	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                      |
	| Text  | Equals | Please enter your postcode |
	| Href  | Equals | #Address_Postcode          |

	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                     |
	| Text  | Equals | Please enter phone number |
	| Href  | Equals | #PhoneNumber              |
