using System;
using Ravitej.Automation.Common.PageObjects.Exceptions;
using Ravitej.Automation.Common.PageObjects.Session;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Ravitej.Automation.Common.PageObjects.Components.BusyIndicator.Selenium
{
    public class ProcessingSpinner : IProcessingSpinner
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _container;
        private readonly By _locator;

        public ProcessingSpinner(ISession session, By processingSpinnerLocator)
        {
            Name = "Processing spinner";
            _locator = processingSpinnerLocator;
            _driver = session.DriverSession.Driver;
            _container = _driver.FindElementSafe(_locator);
        }
        
        public bool Wait(TimeSpan timeout)
        {
            var webDriverWait = new WebDriverWait(_driver, timeout);
            return webDriverWait.Until(d => IsDisplayed() == false);
        }

        public string Name { get; set; }

        /// <summary>
        /// Gets a value indicating whether a processing spinner is displayed or not.
        /// </summary>
        /// <param name="throwWhenNotDisplayed">Indicate <see cref="NotOnPageException"/> should be thrown if not displayed.</param>
        /// <returns>True if a processing spinner is displayed. False if not displayed and <paramref name="throwWhenNotDisplayed"/> is set to false.</returns>
        /// <throws></throws>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="NotOnPageException">Thrown when the expected processing spinner is not displayed on the page and <paramref name="throwWhenNotDisplayed"/> is set to true.</exception>
        public bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            var isDisplayed = _container != null && _container.Displayed;
            if (!isDisplayed && throwWhenNotDisplayed)
            {
                throw new NotOnPageException(
                    $"The expected processing spinner is currently not displayed. Element locator: {_locator}");
            }
            return isDisplayed;
        }
    }
}