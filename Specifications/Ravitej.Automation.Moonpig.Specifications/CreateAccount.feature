@Create_Account
Feature: Create Account
	In order to place and track orders
	As a moonpig.com customer
	I want to create an account

@Validations
Scenario: When Firstname is missing expected error should be displayed
	Given I have entered Aluru in the surname field
	And I have entered ravitej.aluru@gmail.com in the email address fied
	And I have entered SecretPassword1 in the password field
	When I click Create account button
	Then only one error message should be displayed on the screen
	And the error message should be Please enter your first name.

@Validations
Scenario: When Firstname is missing expected error should be displayed data trail
	Given I have entered following data
		|Field			|Value					|
		|Surname		|Aluru					|
		|Email Address	|ravitej.aluru@gmail.com|
		|Password		|SecretPassword1		|
	When I click Create account button
	Then only one error message should be displayed on the screen
	And the error message should be Please enter your first name.

@Validations
Scenario: When Surname is missing expected error should be displayed
	Given I have entered Ravitej in the firstname field
	And I have entered ravitej.aluru@gmail.com in the email address fied
	And I have entered SecretPassword1 in the password field
	When I click Create account button
	Then only one error message should be displayed on the screen
	And the error message should be Please enter your last name.

@Validations
Scenario: When Password is missing expected error should be displayed
	Given I have entered Ravitej in the firstname field
	And I have entered Aluru in the surname field
	And I have entered ravitej.aluru@gmail.com in the email address fied
	When I click Create account button
	Then only one error message should be displayed on the screen
	And the error message should be Enter 6 characters or more including a letter and a number

@Validations
Scenario: When Email address is missing expected error should be displayed
	Given I have entered Ravitej in the firstname field
	And I have entered Aluru in the surname field
	And I have entered SecretPassword1 in the password field
	When I click Create account button
	Then only one error message should be displayed on the screen
	And the error message should be Please enter a valid email address.
