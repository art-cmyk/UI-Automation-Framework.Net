using System;
using System.Linq;
using OpenQA.Selenium;
using Ravitej.Automation.Common.Config.SuiteSettings;
using Ravitej.Automation.Common.Drivers.CapabilityProviders;
using Ravitej.Automation.Common.Config.DriverSession;
using Ravitej.Automation.Common.DriverFactories;
using Logs = Ravitej.Automation.Common.Logging.Logging;

namespace Ravitej.Automation.Common.Drivers
{
    /// <summary>
    /// Provides access to the <see cref="IWebDriver"/> instance and its settings which is used to drive the browser.
    /// </summary>
    public class WebDriverSession : IDriverSession
    {
        #region Fields

        /// <summary>
        /// The settings applicable to the current session and its webdriver instance.
        /// See <see cref="DriverSettings"/> for details of the applicable settings.
        /// </summary>
        public ISuiteSettings SuiteSettings { get; private set; }

        /// <summary>
        /// The webdriver instance which drives the browser.
        /// </summary>
        public IWebDriver Driver { get; private set; }

        #endregion

        #region Constructors and Finalisers
        
        /// <summary>
        /// Initialises a new instance of <see cref="WebDriverSession"/>. 
        /// Creates a new instance of <see cref="IWebDriver"/> using the settings from
        /// <paramref name="suiteSettings"/> and launches the browser ready to 
        /// execute a suite of tests.
        /// </summary>
        /// <param name="driverFactory">The instance of <see cref="IDriverFactory"/>"/> used to create the driver instance.</param>
        /// <param name="suiteSettings">The settings to assign to the new session and its driver.</param>
        public WebDriverSession(IDriverFactory driverFactory, ISuiteSettings suiteSettings)
        {
            SuiteSettings = suiteSettings;
            
            var capabilityProvider = CapabilityFactory.Provider(SuiteSettings.WebDriverSettings);
            var hubUrl = new Uri(SuiteSettings.WebDriverSettings.HubUrl);
            foreach (var capability in SuiteSettings.WebDriverSettings.AdditionalCapabilities.Where(capability => capability.Id != "Sample"))
            {
                capabilityProvider.SetAdditionalCapability(capability);
            }
            var finalCapabilities = capabilityProvider.FinalizeCapabilities();

            var driverTimeouts = new DriverTimeouts(SuiteSettings.WebDriverSettings.ImplicitWaitSeconds, suiteSettings.WebDriverSettings.ScriptTimeoutSeconds, suiteSettings.WebDriverSettings.PageLoadTimeoutSeconds, suiteSettings.WebDriverSettings.CommandTimeoutSeconds);
            Driver = driverFactory.Create(hubUrl, finalCapabilities, driverTimeouts.CommandTimeout);
            Driver = driverFactory.SetTimeouts(Driver, driverTimeouts);

            if (SuiteSettings.WebDriverSettings.MaximiseBrowser)
            {
                Driver.Manage().Window.Maximize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ~WebDriverSession()
        {
            _Dispose(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _Dispose(false);
        }

        private void _Dispose(bool bDisposing)
        {
            if (bDisposing)
            {
                GC.SuppressFinalize(this);
            }
            if (Driver != null)
            {
                _BeforeDispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void _BeforeDispose()
        {
        }

        #endregion Constructors and Finalisers

        /// <summary>
        /// Starts the session by navigating to the given <param name="url"/>.
        /// </summary>
        /// <param name="url">Url of the page to navigate to</param>
        public void Start(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Driver.Navigate().GoToUrl(url);
                Logs.WriteInfoLogEntry(GetType().ToString(), "Started new driver session with url: {0}", url);
            }
            else
            {
                Logs.WriteErrorLogEntry(GetType().ToString(), "Attempt to start the driver session failed. The url cannot be null or empty string. Please specify a valid url.");
                throw new ArgumentException("The url cannot be null or empty string. Please specify a valid url.", nameof(url));
            }
        }

        /// <summary>
        /// Deletes all cookies from the current session.
        /// </summary>
        public void DeleteAllCookies()
        {
            Driver.Manage().Cookies.DeleteAllCookies();
            Logs.WriteInfoLogEntry(GetType().ToString(), "Deleted all cookies from the current driver session.");
        }
    }
}
