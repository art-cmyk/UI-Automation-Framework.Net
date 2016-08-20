using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Session.Navigators;

namespace Ravitej.Automation.Sample.PageObjects.Fluent
{
    public class MoonpigSessionNavigatorGoTo : GoTo
    {
        public MoonpigSessionNavigatorGoTo(IDriverSession driverSession)
            : base(driverSession)
        {
        }
    }
}