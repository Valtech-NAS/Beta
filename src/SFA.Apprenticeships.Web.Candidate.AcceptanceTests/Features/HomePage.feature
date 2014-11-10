﻿Feature: HomePage
	In order to check that I am on the home page
	As a candidate user
	I want to check the header of the page

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@pageload @SmokeTests
Scenario: Home Page Test Scenario
	Then I wait to see Header

@pagenavigation @SmokeTests
Scenario: Vacancy Search Navigation Test
	Given I navigated to the HomePage page
	When I choose VacancySearchLink
	Then I wait for the VacancySearchPage page