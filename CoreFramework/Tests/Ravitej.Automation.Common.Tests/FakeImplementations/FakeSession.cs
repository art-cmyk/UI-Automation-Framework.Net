using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Common.PageObjects.Session.Navigators;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeSession : ISession
    {
        public FakeSession()
        {
            DriverSession = new WebDriverSession(new FakeWebDriverFactory(new FakeWebDriver()), new FakeSuiteSettings());
        }

        public IDriverSession DriverSession { get; private set; }
        public GoTo GoTo { get; private set; }
        public OnPage OnPage { get; private set; }
        public void Quit()
        {
            throw new System.NotImplementedException();
        }
    }


}