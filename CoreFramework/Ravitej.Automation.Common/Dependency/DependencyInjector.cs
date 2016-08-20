using Ravitej.Automation.Common.PageObjects.Session.Navigators;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.DriverFactories;
using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Components.Menu;
using Ravitej.Automation.Common.PageObjects.Components.Menu.Selenium;

namespace Ravitej.Automation.Common.Dependency
{
    /// <summary>
    /// Base class for the dependency injector
    /// </summary>
    public abstract class DependencyInjector
    {
        /// <summary>
        /// Registers necessary types with the dependency container
        /// </summary>
        public void InjectRegistrations()
        {
            //Drivers
            AppWide.Instance.RegisterType<IWebDriver, RemoteWebDriver>();
            AppWide.Instance.RegisterType<IWebDriver, AndroidDriver<IWebElement>>();
            AppWide.Instance.RegisterType<IWebDriver, IOSDriver<IWebElement>>();
            AppWide.Instance.RegisterType<IWebDriver, ChromeDriver>();
            AppWide.Instance.RegisterType<IWebDriver, FirefoxDriver>();
            
            //Factories
            AppWide.Instance.RegisterType<IDriverFactory, RemoteWebDriverFactory>();
            AppWide.Instance.RegisterType<IDriverFactory, IOSDriverFactory>();
            AppWide.Instance.RegisterType<IDriverFactory, AndroidDriverFactory>();
            AppWide.Instance.RegisterType<IDriverFactory, ChromeDriverFactory>();
            AppWide.Instance.RegisterType<IDriverFactory, AndroidDriverFactory>();
            
            //Sessions
            AppWide.Instance.RegisterType<IDriverSession, WebDriverSession>();

            AppWide.Instance.RegisterType<PageObjects.Components.AlertDialog.IAlertDialog, PageObjects.Components.AlertDialog.Selenium.AlertDialog>();
            AppWide.Instance.RegisterType<PageObjects.Components.BusyIndicator.IProcessingSpinner, PageObjects.Components.BusyIndicator.Selenium.ProcessingSpinner>();
            AppWide.Instance.RegisterType<PageObjects.Components.ModalDialog.IModalDialog, PageObjects.Components.ModalDialog.Selenium.ModalDialog>();
            AppWide.Instance.RegisterType<PageObjects.Components.SlidingPanel.ISlidingPanel, PageObjects.Components.SlidingPanel.Selenium.SlidingPanel>();
            AppWide.Instance.RegisterType<INavigator, Navigator>();
            AppWide.Instance.RegisterType<IMenu, Menu>();
            RegisterSiteComponents();
            RegisterPageObjects();
        }

        /// <summary>
        /// Entry point to perform all page object registrations
        /// </summary>
        protected abstract void RegisterPageObjects();

        /// <summary>
        /// Entry point to register any explicit site components, such as the BusyIndicator etc.
        /// </summary>
        protected abstract void RegisterSiteComponents();
    }
}
