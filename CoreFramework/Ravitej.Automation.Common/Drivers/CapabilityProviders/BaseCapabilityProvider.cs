using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.Drivers.CapabilityProviders
{
    internal abstract class BaseCapabilityProvider : ICapabilityProvider
    {
        private readonly DriverSettings _driverSettings;
        protected DesiredCapabilities Capabilities;

        protected BaseCapabilityProvider(DriverSettings driverSettings)
        {
            _driverSettings = driverSettings;
        }

        protected void BrowserDefaults(Browser eBrowser)
        {
            switch (eBrowser)
            {
                case Browser.Chrome:
                    {
                        Capabilities = DesiredCapabilities.Chrome();
                        //creating custom profile to avoid download file dialogs etc.
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AddUserProfilePreference("download.default_directory", _driverSettings.DownloadDirectory);
                        chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                        chromeOptions.AddUserProfilePreference("disable-popup-blocking", true);
                        Capabilities = chromeOptions.ToCapabilities() as DesiredCapabilities;
                        Capabilities.SetCapability(CapabilityType.UnexpectedAlertBehavior, "ignore");
                        break;
                    }
                case Browser.Firefox:
                    {
                        Capabilities = DesiredCapabilities.Firefox();
                        Capabilities.SetCapability(CapabilityType.UnexpectedAlertBehavior, "ignore");
                        //creating custom profile to avoid download file dialogs etc.
                        var profile = new FirefoxProfile();
                        profile.SetPreference("browser.download.dir", _driverSettings.DownloadDirectory);
                        profile.SetPreference("browser.download.folderList", 2);
                        profile.SetPreference("browser.helperApps.neverAsk.saveToDisk",
                            "image/jpeg, application/pdf, application/octet-stream");
                        profile.SetPreference("pdfjs.disabled", true);
                        //setting the custom profile into the capabilities.
                        Capabilities.SetCapability(FirefoxDriver.ProfileCapabilityName, profile.ToBase64String());
                        break;
                    }
                case Browser.Opera:
                    {
                        Capabilities = DesiredCapabilities.Opera();
                        Capabilities.SetCapability(CapabilityType.UnexpectedAlertBehavior, "ignore");
                        break;
                    }
                case Browser.InternetExplorer:
                    {
                        Capabilities = DesiredCapabilities.InternetExplorer();
                        Capabilities.SetCapability("EnableNativeEvents", true);
                        Capabilities.SetCapability("EnablePersistentHover", true);
                        Capabilities.SetCapability("RequireWindowFocus", true);
                        Capabilities.SetCapability(CapabilityType.UnexpectedAlertBehavior, "ignore");
                        break;
                    }
                case Browser.Edge:
                    {
                        Capabilities = DesiredCapabilities.Edge();
                        Capabilities.SetCapability(CapabilityType.UnexpectedAlertBehavior, "ignore");
                        break;
                    }
                case Browser.Safari:
                    {
                        Capabilities = DesiredCapabilities.Safari();
                        Capabilities.SetCapability(CapabilityType.UnexpectedAlertBehavior, "ignore");
                        Capabilities.IsJavaScriptEnabled = true;
                        break;
                    }
                case Browser.Unspecified:
                    {
                        Capabilities = new DesiredCapabilities();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Unexpected browser specified");
                    }
            }
        }

        public void SetAdditionalCapability(AdditionalCapability additionalCapability)
        {
            Capabilities.SetCapability(additionalCapability.Id, additionalCapability.Value);
        }

        public abstract DesiredCapabilities FinalizeCapabilities();
    }
}
