using System.Linq;
using NUnit.Framework;
using Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount;

namespace Ravitej.Automation.Sample.Ui.Tests.Tests
{
    [TestFixture]
    public class WhenCreatingAccount : MoonpigTestBase<CreateAccountSettings>
    {
        private ICreateAcount _createAccountPage;

        #region SetUp & TearDown

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            WriteInfoLogEntry(beginTestFixtureSetUp(GetType().Name));
            _createAccountPage = MoonpigSession.OnPage.MoonpigHome.TopBar.ClickCreateAccount();
            WriteInfoLogEntry(endTestFixtureSetUp(GetType().Name));
        }

        [SetUp]
        public void SetUp()
        {
            WriteInfoLogEntry(beginTestSetUp(TestContext.CurrentContext.Test.Name));
            _createAccountPage = _createAccountPage.TopBar.ClickCreateAccount();
            WriteInfoLogEntry(endTestSetUp(TestContext.CurrentContext.Test.Name));
        }

        [TearDown]
        public void TearDown()
        {
            WriteInfoLogEntry(beginTestTearDown(TestContext.CurrentContext.Test.Name));
            WriteInfoLogEntry(endTestTearDown(TestContext.CurrentContext.Test.Name));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            WriteInfoLogEntry(beginTestFixtureTearDown(GetType().Name));
        }
        #endregion

        [Test]
        public void UserShouldBeOnCreateAccountPage()
        {
            Assert.That(_createAccountPage.IsDisplayed(), Is.True, "Expected to be on Create Account page, but was not.");
        }

        [Test]
        public void ClickingCreateAccountButtonWithoutEnteringDetailsShouldDisplayValidationErrors()
        {
            var validationErrorsCount = _createAccountPage.ClickCreateAccount<ICreateAcount>().GetValidationErrorsCount();
            Assert.That(validationErrorsCount, Is.GreaterThan(0), "Expected at least one validation error to be displayed, but was not.");
        }

        [Test]
        public void ClickingCreateAccountButtonWithoutEnteringFirstNameShouldDisplayCorrectValidationError()
        {
            var validationErrors =
                _createAccountPage.EnterSurname(TestSettings.Surname)
                    .EnterEmailAddress(TestSettings.EmailAddress)
                    .EnterPassword(TestSettings.Password)
                    .ClickCreateAccount<ICreateAcount>()
                    .GetValidationErrors().ToList();
            //Assert.That(validationErrors.Count(), Is.EqualTo(1), "Expected only one validation error to be displayed.");
            Assert.That(validationErrors.Contains(TestSettings.FirstNameValidationMessage), Is.True,
                $"Expected validation message to be {TestSettings.FirstNameValidationMessage}, but was {validationErrors.ElementAt(0)}.");
        }

        [Test]
        public void ClickingCreateAccountButtonWithoutEnteringFirstNameShouldDisplayOnlyOneValidationError()
        {
            var validationErrorsCount =
                _createAccountPage.EnterSurname(TestSettings.Surname)
                    .EnterEmailAddress(TestSettings.EmailAddress)
                    .EnterPassword(TestSettings.Password)
                    .ClickCreateAccount<ICreateAcount>()
                    .GetValidationErrorsCount();
            Assert.That(validationErrorsCount, Is.EqualTo(1), "Expected only one validation error to be displayed.");
        }
    }
}
