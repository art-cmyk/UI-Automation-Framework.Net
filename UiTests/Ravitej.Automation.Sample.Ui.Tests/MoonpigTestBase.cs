using NUnit.Framework;
using Ravitej.Automation.Common.Config;
using Ravitej.Automation.Common.Dependency;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Sample.PageObjects.DependencyContainer;
using Ravitej.Automation.Sample.PageObjects.Fluent;
using Ravitej.Automation.Sample.Ui.Tests.Settings;
using Ravitej.Automation.UI.Tests;

namespace Ravitej.Automation.Sample.Ui.Tests
{
    public class MoonpigTestBase<TSettings> : UiTestBase<MoonpigSuiteSettings> where TSettings : IPersistableSettings, new()
    {
        private readonly SettingsType _settingsType;

        protected MoonpigTestBase(SettingsType settingsType = SettingsType.ProjectBound)
        {
            TestBaseNamespace = "Ravitej.Automation.Sample.Ui.Tests";
            TestResultsBaseFolder = "Moonpig";
            _settingsType = settingsType;
        }

        protected MoonpigTestBase(LaunchPage launchTarget, SettingsType settingsType = SettingsType.ProjectBound)
            : base((int)launchTarget)
        {
            TestBaseNamespace = "Ravitej.Automation.Sample.Ui.Tests";
            TestResultsBaseFolder = "Moonpig";
            _settingsType = settingsType;
        }

        protected IMoonpigSession MoonpigSession { get; private set; }

        protected TSettings TestSettings { get; set; }

        protected void NavigateToSite(LaunchPage targetPage, bool handleNavigateAwayWarning = true)
        {
            NavigateToLaunchSite((int)targetPage, handleNavigateAwayWarning);
        }

        [OneTimeSetUp]
        protected void MoonpigTestBaseFixtureSetUp()
        {
            new MoonpigDependencyInjector().InjectRegistrations(); //registers types with the container
            MoonpigSession = new MoonpigSession(Session.DriverSession);
            AppWide.Instance.SetInstance(MoonpigSession);
            AppWide.Instance.SetInstance<ISession>(MoonpigSession);
            TestSettings = GetTestSettingsAndPersistIfDefault<TSettings>(_settingsType);
        }

        [SetUp]
        protected void MoonpigTestBaseSetUp()
        {

        }
        [TearDown]
        protected void MoonpigTestBaseTearDown()
        {

        }

        [OneTimeTearDown]
        protected void MoonpigTestBaseFixtureTearDown()
        {

        }
    }
}
