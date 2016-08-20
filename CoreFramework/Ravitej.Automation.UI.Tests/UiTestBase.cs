using Ravitej.Automation.Common.Config.SuiteSettings;
using Ravitej.Automation.Common.Tests;

namespace Ravitej.Automation.UI.Tests
{
    /// <summary>
    /// Base class for all UI tests
    /// </summary>
    /// <typeparam name="TSuiteSettingsType"></typeparam>
    public abstract class UiTestBase<TSuiteSettingsType> : TestBase<TSuiteSettingsType> where TSuiteSettingsType : ISuiteSettings, new()
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        protected UiTestBase()
        {
            TestBaseNamespace = "Ravitej.Automation.UI.Tests";
            TestResultsBaseFolder = "";
        }

        /// <summary>
        /// Overloaded constructor taking in the target page to launch
        /// </summary>
        /// <param name="launchTarget"></param>
        protected UiTestBase(int launchTarget)
            : base(launchTarget)
        {
            TestBaseNamespace = "Ravitej.Automation.UI.Tests";
            TestResultsBaseFolder = "";
        }

        /// <summary>
        /// Overloaded constructor taking in the target page to launch 
        /// and the username in case of basic authentication
        /// </summary>
        /// <param name="launchTarget"></param>
        /// <param name="basicAuthUsername"></param>
        protected UiTestBase(int launchTarget, string basicAuthUsername)
            : base(launchTarget, basicAuthUsername)
        {
            TestBaseNamespace = "Ravitej.Automation.UI.Tests";
            TestResultsBaseFolder = "";
        }
    }
}
