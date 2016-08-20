using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Session.Navigators;

namespace Ravitej.Automation.Common.PageObjects.Session
{
    public interface ISession
    {
        IDriverSession DriverSession { get; }
        GoTo GoTo { get; }
        OnPage OnPage { get; }
        void Quit();
    }
}