@US712
Feature: Helpdesk
	As the SFA 
	we want to offer the candidate a web form to contact us through
	so we can receive more structured contact requests and provide a convenient mechanism for the candidate to contact us through

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

Scenario: Contact form
	Given I navigated to the HelpdeskPage page
	When I am on the HelpdeskPage page
	And I choose SendButton
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 3     |
	When I am on the HelpdeskPage page
	And I enter data
		| Field | Value             |
		| Name  | FirstnameTest     |
		| Email | someone@gmail.com |
	When I am on the HelpdeskPage page
	And I am on EnquiryDropdown list item matching criteria
		| Field | Rule   | Value                             |
		| Text  | Equals | I need to change my email address |
	And I choose WrappedElement
	And I am on the HelpdeskPage page
	Then I see 
		| Field   | Rule   | Value                             |
		| Enquiry | Equals | I need to change my email address |
	When I am on the HelpdeskPage page
	And I am on EnquiryDropdown list item matching criteria
		| Field | Rule   | Value                                       |
		| Text  | Equals | -- Or choose from one of these questions -- |
	And I choose WrappedElement
	And I am on the HelpdeskPage page
	Then I see 
		| Field   | Rule   | Value |
		| Enquiry | Equals |       |
	When I am on the HelpdeskPage page
	And I enter data
		| Field   | Value      |
		| Enquiry | An enquiry |
	Then I see 
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 0     |