using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.DriverFactories
{
    /// <summary>
    /// Factory interface to the IWebDriver
    /// </summary>
    public interface IDriverFactory
    {
        /// <summary>
        /// Create a new instance of an IWebDriver using the specified hub url, capabilities and command timeout
        /// </summary>
        /// <param name="hubUrl"></param>
        /// <param name="capabilities"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities, TimeSpan commandTimeout);

        /// <summary>
        /// Creates a new instance of IWebDriver using the specified hub url and capabilities
        /// </summary>
        /// <param name="hubUrl"></param>
        /// <param name="capabilities"></param>
        /// <returns></returns>
        IWebDriver Create(Uri hubUrl, DesiredCapabilities capabilities);

        /// <summary>
        /// Sets the given implicit wait, script timeout and page load timeout values to the given instance 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeouts"></param>
        /// <returns></returns>
        IWebDriver SetTimeouts(IWebDriver driver, DriverTimeouts timeouts);
    }
}