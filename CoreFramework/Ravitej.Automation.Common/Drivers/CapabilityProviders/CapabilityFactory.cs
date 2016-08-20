using System.ComponentModel;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.Drivers.CapabilityProviders
{
    internal static class CapabilityFactory
    {
        public static ICapabilityProvider Provider(DriverSettings driverSettings)
        {
            switch (driverSettings.HubType.EnumValue)
            {
                case HubType.BrowserStack:
                {
                    return new BrowserStackCapabilityProvider(driverSettings);
                }
                case HubType.SauceLabs:
                {
                    return new SauceLabsCapabilityProvider(driverSettings);
                }
                case HubType.CrossBrowserTesting:
                {
                    return new CrossBrowserTestingCapabilityProvider(driverSettings);
                }
                case HubType.Internal:
                {
                    return new InternalCapabilityProvider(driverSettings);
                }
                case HubType.None:
                {
                    return null;
                }
                default:
                    throw new InvalidEnumArgumentException(
                        $"Accepted values are: {string.Join(", ", driverSettings.HubType.PermittedValues.ToArray())}");
            }
        }
    }
}
