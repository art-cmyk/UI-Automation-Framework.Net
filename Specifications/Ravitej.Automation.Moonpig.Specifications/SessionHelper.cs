using Ravitej.Automation.Sample.PageObjects.Fluent;
using Ravitej.Automation.Sample.Ui.Tests;
using Ravitej.Automation.Sample.Ui.Tests.Settings;

namespace Ravitej.Automation.Moonpig.Specifications
{
    public class SessionHelper : MoonpigTestBase<MoonpigPersistableSettings>
    {
        public IMoonpigSession StartSession()
        {
            TestBaseFixtureSetUp();
            MoonpigTestBaseFixtureSetUp();
            return MoonpigSession;
        }

        public void EndSession()
        {
            MoonpigTestBaseFixtureTearDown();
            TestBaseFixtureTearDown();
        }
    }
}
