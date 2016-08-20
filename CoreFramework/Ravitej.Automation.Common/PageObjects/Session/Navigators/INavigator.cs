using Ravitej.Automation.Common.Drivers;

namespace Ravitej.Automation.Common.PageObjects.Session.Navigators
{
    public interface INavigator
    {
        IDriverSession DriverSession { get; set; }
    }
}