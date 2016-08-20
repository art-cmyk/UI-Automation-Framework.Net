using OpenQA.Selenium;
using Ravitej.Automation.Common.Config.SuiteSettings;

namespace Ravitej.Automation.Common.Drivers
{
    public interface IDriverSession
    {
        ISuiteSettings SuiteSettings { get; }
        IWebDriver Driver { get; }
        void Start(string url);
        void DeleteAllCookies();
        void Dispose();
    }
}