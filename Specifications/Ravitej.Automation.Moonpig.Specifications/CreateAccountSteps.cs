using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ravitej.Automation.Sample.PageObjects.Fluent;
using Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount;
using TechTalk.SpecFlow;

namespace Ravitej.Automation.Moonpig.Specifications
{
    [Binding]
    public class CreateAccountSteps
    {
        private static ICreateAcount _createAccountPage;
        private IEnumerable<string> _validationErrors;
        private static IMoonpigSession _moonpigSession;
        private static SessionHelper _sessionHelper;

        [BeforeFeature()]
        public static void BeforeFeature()
        {
            //var testName = $"{FeatureContext.Current.FeatureInfo.Title}.{ScenarioContext.Current.ScenarioInfo.Title}";
            _sessionHelper = new SessionHelper();
            _moonpigSession = _sessionHelper.StartSession();
            _createAccountPage = _moonpigSession.OnPage.MoonpigHome.TopBar.ClickCreateAccount();
        }

        [BeforeScenario()]
        public static void BeforeScenario()
        {
            _createAccountPage.TopBar.ClickCreateAccount();
        }

        [AfterScenario()]
        public static void AfterScenario()
        {
        }

        [AfterFeature()]
        public static void AfterFeature()
        {
            _sessionHelper.EndSession();
        }

        [Given]
        public void GivenIHaveEntered_SURNAME_InTheSurnameField(string surname)
        {
            _createAccountPage.EnterSurname(surname);
        }

        [Given]
        public void GivenIHaveEntered_FIRSTNAME_InTheFirstnameField(string firstname)
        {
            _createAccountPage.EnterFirstName(firstname);
        }

        [Given]
        public void GivenIHaveEntered_EMAILADDRESS_InTheEmailAddressFied(string emailAddress)
        {
            _createAccountPage.EnterEmailAddress(emailAddress);
        }

        [Given]
        public void GivenIHaveEntered_PASSWORD_InThePasswordField(string password)
        {
            _createAccountPage.EnterPassword(password);
        }

        [Given]
        public void GivenIHaveEnteredFollowingData(Table table)
        {
            foreach(var row in table.Rows)
            {
                if(row["Field"].Equals("Surname"))
                {
                    _createAccountPage.EnterSurname(row["Value"].ToString());
                    continue;
                }
                if (row["Field"].Equals("Firstname"))
                {
                    _createAccountPage.EnterFirstName(row["Value"].ToString());
                    continue;
                }
                if (row["Field"].Equals("Email Address"))
                {
                    _createAccountPage.EnterEmailAddress(row["Value"].ToString());
                    continue;
                }
                if (row["Field"].Equals("Password"))
                {
                    _createAccountPage.EnterPassword(row["Value"].ToString());
                    continue;
                }
            }
        }

        [When]
        public void WhenIClickCreateAccountButton()
        {
            _createAccountPage = _createAccountPage.ClickCreateAccount<ICreateAcount>();
        }
        
        [Then]
        public void ThenOnlyOneErrorMessageShouldBeDisplayedOnTheScreen()
        {
            _validationErrors = _createAccountPage.GetValidationErrors().ToList();
            Assert.That(_validationErrors.Count(), Is.EqualTo(1),
                $"Expected only 1 validation error message, but there were {_validationErrors.Count()}");
            
        }
        
        [Then]
        public void ThenTheErrorMessageShouldBe_ERRORMESSAGE(string errorMessage)
        {
            Assert.That(_validationErrors.Contains(errorMessage), $"Expected error message '{errorMessage}', but was not found.");
        }
    }
}
