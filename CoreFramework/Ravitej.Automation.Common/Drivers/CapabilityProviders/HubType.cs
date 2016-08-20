namespace Ravitej.Automation.Common.Drivers.CapabilityProviders
{
    /// <summary>
    /// The type of hub being used
    /// </summary>
    public enum HubType
    {
        /// <summary>
        /// No hub; browser is started locally
        /// </summary>
        None,
        /// <summary>
        /// The internal remote hub
        /// </summary>
        Internal,

        /// <summary>
        /// BrowserStack.com cloud hub
        /// </summary>
        BrowserStack,

        /// <summary>
        /// SauceLabs on demand cloud hub
        /// </summary>
        SauceLabs,

        /// <summary>
        /// CrossBrowserTesting.com cloud hub 
        /// </summary>
        CrossBrowserTesting
    }
}