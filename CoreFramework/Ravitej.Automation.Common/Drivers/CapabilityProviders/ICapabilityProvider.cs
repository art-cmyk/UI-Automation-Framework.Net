using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.Drivers.CapabilityProviders
{
    internal interface ICapabilityProvider
    {
        void SetAdditionalCapability(AdditionalCapability additionalCapability);

        DesiredCapabilities FinalizeCapabilities();
    }
}
