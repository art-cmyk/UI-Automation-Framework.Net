using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Applitools;
using NUnit.Framework;
using OpenQA.Selenium;
using Platform = Ravitej.Automation.Common.Config.DriverSession.Platform;
using Ravitej.Automation.Sample.Ui.Tests;
using Ravitej.Automation.Common.Config.DriverSession;
using Ravitej.Automation.Common.Config;
using Ravitej.Automation.Sample.Ui.Tests.Settings;
using Ravitej.Automation.Common.Tests;

namespace Adp.Automation.Run.UI.Tests.Visual
{
    [TestFixture]
    public class ExperimentalVisualTests : MoonpigTestBase<ExperimentalVisualTestSettings>
    {
        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            WriteInfoLogEntry(beginTestFixtureSetUp(GetType().Name));
            WriteInfoLogEntry(endTestFixtureSetUp(GetType().Name));
        }

        [SetUp]
        public void SetUp()
        {
            WriteInfoLogEntry(beginTestSetUp(TestContext.CurrentContext.Test.Name));
            WriteInfoLogEntry(endTestSetUp(TestContext.CurrentContext.Test.Name));
        }

        [TearDown]
        public void TearDown()
        {
            WriteInfoLogEntry(beginTestTearDown(TestContext.CurrentContext.Test.Name));
            WriteInfoLogEntry(endTestTearDown(TestContext.CurrentContext.Test.Name));
        }

        [Test, Ignore("")]
        public void AppliToolsSampleTest()
        {
            // This is your api key, make sure you use it in all your tests.
            var eyes = new Eyes { ApiKey = "PdSZ1clWMAgFaIv97WZ6Uwrw3U03LT0lYFAbhAmiEw70110" };
            var driver = MoonpigSession.DriverSession.Driver;
            try
            {
                // Start visual testing with browser viewport set to 1024x768.
                // Make sure to use the returned driver from this point on.
                //eyes.SetAppEnvironment(null, "all browsers");
                eyes.MatchLevel = MatchLevel.Layout;
                eyes.SaveNewTests = true;
                driver = eyes.Open(driver, "Applitools", "Test Web Page", new Size(1920, 1080));


                driver.Navigate().GoToUrl("http://www.applitools.com");

                // Visual validation point #1
                eyes.CheckWindow("Main Page");

                driver.FindElement(By.CssSelector(".features>a")).Click();

                // Visual validation point #2
                eyes.CheckWindow("Features Page");

                // End visual testing. Validate visual correctness.
                eyes.Close();
            }
            finally
            {
                eyes.AbortIfNotClosed();
                //driver.Quit();
            }
        }

        [Test] [Combinatorial]
        public void VisualTestsSample(
            //[Values(Browser.Chrome, Browser.Firefox)] Browser browser,
            [Values(Browser.Chrome)] Browser browser)
            //[Values()] KeyValuePair<int, int> resolution)
            //[ValueSource("Resolutions")] KeyValuePair<int, int> resolution)
        {
            // This is your api key, make sure you use it in all your tests.
            var eyes = new Eyes { ApiKey = "6Lv5UFTHTXMCYambSxjAyD0UWzKK110YKR2Sjv103c105zuSs110" };
            var session = StartUpSession(browser, "50.0", Platform.Windows, "7", TestSuiteSettings.ApplicationUnderTestSettings.Url);
            var driver = session.DriverSession.Driver;
            //var x = new KeyValuePair<int, int>[] { new KeyValuePair<int, int>(441, 326), new KeyValuePair<int, int>(800, 600) };
            try
            {
                // Start visual testing with browser viewport set to 1024x768.
                // Make sure to use the returned driver from this point on.
                driver = eyes.Open(driver, "Beamly Agency Production Site", "Home page",
                    new Size(414, 706));
                eyes.MatchLevel = MatchLevel.Layout2;

                driver.Navigate().GoToUrl("http://www.beamly.com");
                // Visual validation point #1
                eyes.CheckWindow("Beamly home page");

                ////driver.FindElement(By.CssSelector(".features>a")).Click();
                //var associateHome = AssociateLogin.PerformAssociateLogin(RunSession, TestSettings.UserName,
                //    TestSettings.Password);

                //// Visual validation point #2
                //eyes.CheckWindow("Associate Home page soon after login");

                //associateHome.EnterIidOrCompanyName("submission cpa")
                //    .SelectSearchOption(SearchOption.CompanyName)
                //    .ClickFilterList();

                //// Visual validation point #3
                //eyes.CheckWindow("Associate Home page after searching for 'submission cpa' company");

                //var clientList = associateHome.ClickResult<IClientList>(SearchOption.Iid, "20063215");

                //// Visual validation point #4
                //eyes.CheckWindow("Client List page for CPA IID = 20063215");

                // End visual testing. Validate visual correctness.
                eyes.Close();
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw;
            }
            finally
            {
                eyes.AbortIfNotClosed();
                session.Quit();
            }
        }

        //private static IEnumerable Resolutions()
        //{
        //    var testSettings = GetTestSettingsAndPersistIfDefault<ExperimentalVisualTestSettings>(SettingsType.ProjectBound);
        //    var heights = testSettings.Heights;
        //    var widths = testSettings.Widths;
        //    //yield return new KeyValuePair<int, int>(1920, 1200); //for some reason Eyes fails to set this viewport size.
        //    return heights.Select((t, i) => new KeyValuePair<int, int>(widths[i], t));
        //}
    }
}
