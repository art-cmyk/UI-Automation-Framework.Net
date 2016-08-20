using Ravitej.Automation.Common.Config.AppUnderTest;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.Config.SuiteSettings
{
    /// <summary>
    /// Provides the interface through which user can read/write common settings applicable to a suite of tests from/to a config file.
    /// </summary>
    public interface ISuiteSettings : IPersistableSettings
    {
        /// <summary>
        /// Gets the launch page url based on the id specified in <paramref name="targetPage"/>
        /// </summary>
        /// <param name="targetPage"></param>
        /// <returns></returns>
        string GetLaunchPage(int targetPage);

        /// <summary>
        /// Gets the launch page url based on the id specified in <paramref name="targetPage"/> using the logic specified in <paramref name="launchPageHandler"/>
        /// </summary>
        /// <param name="targetPage"></param>
        /// <param name="launchPageHandler"></param>
        /// <returns></returns>
        string GetLaunchPage(int targetPage, ILaunchPageHandler launchPageHandler);

        /// <summary>
        /// Settings applicable to webdriver such as huburl, browser, platform, timeouts etc. as defined by <see cref="DriverSettings"/>.
        /// </summary>
        DriverSettings WebDriverSettings { get; set; }

        /// <summary>
        /// Settings applicable to the application under test (AUT)
        /// </summary>
        AutSettings ApplicationUnderTestSettings { get; set; }
    }
}
