using System;

namespace Ravitej.Automation.Common.Config.DriverSession
{
    /// <summary>
    /// Represents various timeouts applicable to the webdriver instance driving the browser in which the suite of tests are executed.
    /// </summary>
    [Serializable]
    public class DriverTimeouts
    {
        /// <summary>
        /// The timespan defining the implicit wait timeout to assign to the webdriver instance. 
        /// See <see cref="OpenQA.Selenium.ITimeouts.ImplicitlyWait"/> for more information.
        /// </summary>
        public TimeSpan ImplicitWait
        {
            get;
            private set;
        }

        /// <summary>
        /// The timespan defining the script timeout to assign to the webdriver instance. 
        /// See <see cref="OpenQA.Selenium.ITimeouts.SetScriptTimeout"/> for more information.
        /// </summary>
        public TimeSpan ScriptTimeout
        {
            get;
            private set;
        }

        /// <summary>
        /// The timespan defining the page load timeout to assign to the webdriver instance. 
        /// See <see cref="OpenQA.Selenium.ITimeouts.SetPageLoadTimeout"/> for more information.
        /// </summary>
        public TimeSpan PageLoadTimeout
        {
            get;
            private set;
        }

        public TimeSpan CommandTimeout { get; private set; }

        /// <summary>
        /// Initialises a new instance of <see cref="DriverTimeouts"/> using the 
        /// <see cref="TimeSpan"/> values passed in.
        /// </summary>
        /// <param name="implicitWait"></param>
        /// <param name="scriptTimeout"></param>
        /// <param name="pageLoadTimeout"></param>
        /// <param name="commandTimeout"></param>
        public DriverTimeouts(TimeSpan implicitWait, TimeSpan scriptTimeout, TimeSpan pageLoadTimeout, TimeSpan commandTimeout)
        {
            ImplicitWait = implicitWait;
            ScriptTimeout = scriptTimeout;
            PageLoadTimeout = pageLoadTimeout;
            CommandTimeout = commandTimeout;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="DriverTimeouts"/> by converting the 
        /// passed in <see cref="int"/> values to corresponding <see cref="TimeSpan"/> values.
        /// </summary>
        /// <param name="implicitWaitSeconds"></param>
        /// <param name="scriptTimeoutSeconds"></param>
        /// <param name="pageLoadTimeoutSeconds"></param>
        /// <param name="commandTimeoutSeconds"></param>
        public DriverTimeouts(int implicitWaitSeconds, int scriptTimeoutSeconds, int pageLoadTimeoutSeconds, int commandTimeoutSeconds)
        {
            ImplicitWait = ToTimeSpan(implicitWaitSeconds);
            ScriptTimeout = ToTimeSpan(scriptTimeoutSeconds);
            PageLoadTimeout = ToTimeSpan(pageLoadTimeoutSeconds);
            CommandTimeout = ToTimeSpan(commandTimeoutSeconds);
        }

        private static TimeSpan ToTimeSpan(int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }
    }
}
