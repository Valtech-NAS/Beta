﻿Feature: HomePage
	In order to check the site is working
	As a user
	I want to be told the page has loaded

@mytag
Scenario: Home page loads
	Given I navigated to the homepage
	Then the screen has the title of 'Candidate Home page'
