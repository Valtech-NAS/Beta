﻿@US583
Feature: Traineeship Search Validation
	In order to find a traineeship vacancy quickly
	As a candidate
	I want invalid inputs to be highlighted before searching

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@SmokeTests
Scenario: Show validation error message when no location entered
	Given I navigated to the TraineeshipSearchPage page
	When I choose Search
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 1     |