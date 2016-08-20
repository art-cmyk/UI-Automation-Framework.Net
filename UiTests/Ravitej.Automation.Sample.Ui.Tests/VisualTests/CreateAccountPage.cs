using Applitools;
using NUnit.Framework;
using Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount;
using System.Drawing;
using Ravitej.Automation.Sample.Ui.Tests.Tests;

namespace Ravitej.Automation.Sample.Ui.Tests.VisualTests
{
    [TestFixture]
    public class CreateAccountPage : MoonpigTestBase<CreateAccountSettings>
    {
        private ICreateAcount _createAccountPage;
        private Eyes _eyes = new Eyes();
        #region SetUp & TearDown

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            WriteInfoLogEntry(beginTestFixtureSetUp(GetType().Name));
            // This is your api key, make sure you use it in all your tests.
            _eyes.ApiKey = "6Lv5UFTHTXMCYambSxjAyD0UWzKK110YKR2Sjv103c105zuSs110";
            _eyes.Open(MoonpigSession.DriverSession.Driver, "Moonpig Production", "Create Account Page", new Size(800, 600));
            _eyes.MatchLevel = MatchLevel.Layout2;
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
            _eyes.AbortIfNotClosed();
        }
        #endregion

        [Test]
        public void UserShouldBeOnCreateAccountPage()
        {
            // Visual validation point #1
            _eyes.CheckWindow(TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void ClickingCreateAccountButtonWithoutEnteringAnyDetails()
        {
            var validationErrorsCount = _createAccountPage.ClickCreateAccount<ICreateAcount>().GetValidationErrorsCount();
            // Visual validation point after clicking Create account button without entering any details
            _eyes.CheckWindow(TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void ClickingCreateAccountButtonWithoutEnteringFirstName()
        {
            var validationErrors =
                _createAccountPage.EnterSurname(TestSettings.Surname)
                    .EnterEmailAddress(TestSettings.EmailAddress)
                    .EnterPassword(TestSettings.Password)
                    .ClickCreateAccount<ICreateAcount>();
            // Visual validation point after clicking Create account button without entering first name
            _eyes.CheckWindow(TestContext.CurrentContext.Test.Name);
        }
    }
}
