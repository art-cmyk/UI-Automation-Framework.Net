using System;

namespace Ravitej.Automation.Common.Config.DriverSession
{
    /// <summary>
    /// Defines an additional capability setting that will be passed into the browser.  These can include specifics such as a target screen resolution, device
    /// whether to ignore SSL errors, explicit credentials to assign to the driver (such as proxy) etc.
    /// </summary>
    [Serializable]
    public class AdditionalCapability
    {
        /// <summary>
        /// The Id of the capability to set
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// The value to assign to the capability
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}
