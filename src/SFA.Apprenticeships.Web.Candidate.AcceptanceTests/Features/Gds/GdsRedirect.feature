Feature: GDS Start Page redirect
	As a Production user
	When I navigate to the Aprenticeships home page
	Then I am redirected to the GDS Start Page

@SmokeTestsProd
Scenario: As a Production user I am redirected to GDS Start Page from Apprenticeships home page 
	Given I navigated to the HomePage page
	Then I am on the GdsStartPage page
