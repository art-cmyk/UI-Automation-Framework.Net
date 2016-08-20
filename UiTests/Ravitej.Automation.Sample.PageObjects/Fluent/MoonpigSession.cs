using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Session;

namespace Ravitej.Automation.Sample.PageObjects.Fluent
{
    public class MoonpigSession : Session, IMoonpigSession
    {
        public new MoonpigSessionNavigatorGoTo GoTo { get; private set; }
        public new MoonpigSessionNavigatorOnPage OnPage { get; private set; }

        public MoonpigSession(IDriverSession driverSession)
            : base(driverSession)
        {
            OnPage = new MoonpigSessionNavigatorOnPage(driverSession);
            GoTo = new MoonpigSessionNavigatorGoTo(driverSession);
        }
    }
}