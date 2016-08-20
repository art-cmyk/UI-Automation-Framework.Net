using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ravitej.Automation.Common.Config;
using Ravitej.Automation.Common.Helpers;
using Ravitej.Automation.Common.PageObjects.Exceptions;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.PageObjects.Services
{
    /// <summary>
    /// The class that provides methods to check if the browser is displaying expected page or not
    /// </summary>
    public class OnPageChecker : IDisposable
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private readonly IWebDriver _driver;

        private readonly string _pageName;

        private bool _allValid = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OnPageChecker"/>
        /// </summary>
        /// <param name="driver">Instance of <see cref="IWebDriver"/> to use to perform the requested checks</param>
        /// <param name="pageName">Name of the expected page</param>
        public OnPageChecker(IWebDriver driver, string pageName)
        {
            _driver = driver;
            _pageName = pageName;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets a value indicating whether expected page is displayed or not
        /// </summary>
        /// <param name="throwWhenNotOnPage">Whether <see cref="NotOnPageException"/> should be thrown if expected page is not displayed</param>
        /// <returns>True if expected page is displayed, false otherwise</returns>
        /// <exception cref="NotOnPageException">If not on the expected page and <paramref name="throwWhenNotOnPage"/> is true</exception>
        public bool Confirm(bool throwWhenNotOnPage)
        {
            if (_allValid)
            {
                return true;
            }

            var filePath = $"{ExecutionSettings.CurrentTestResultsPath}\\_NotOn{_pageName.Replace(" ", "")}Page";
            ScreenshotHelper.ForceTakeScreenshot(_driver, filePath, _stringBuilder.ToString());

            if (throwWhenNotOnPage)
            {
                throw new NotOnPageException(_stringBuilder.ToString());
            }

            return false;
        }

        /// <summary>
        /// Checks that the browser window title is as expected
        /// </summary>
        /// <param name="windowTitle">Expected window title</param>
        /// <param name="overrideTargetFrameName">Id or name of the frame in which to find the element(s). Use null if element is to be found in the top document</param>
        public OnPageChecker TitleOfPageIs(string windowTitle, string overrideTargetFrameName = null)
        {
            string actualPageTitle;

            bool pageTitleIsValid;

            if (string.IsNullOrWhiteSpace(overrideTargetFrameName))
            {
                actualPageTitle = _driver.Title;
                pageTitleIsValid = actualPageTitle == windowTitle;
            }
            else
            {
                actualPageTitle = _driver.SwitchTo().Frame(overrideTargetFrameName).Title;
                pageTitleIsValid = actualPageTitle == windowTitle;
                _driver.SwitchTo().DefaultContent();
            }

            if (!pageTitleIsValid)
            {
                _stringBuilder.AppendLine(
                    $"Expected title of page {_pageName} to be '{windowTitle}', but was '{actualPageTitle}'.");
            }

            _allValid = pageTitleIsValid && _allValid;
            return this;
        }

        /// <summary>
        /// Checks that the given text exists on page
        /// </summary>
        /// <param name="textToFind">Text to find on the page</param>
        /// <param name="overrideTargetFrameName">Id or name of the frame in which to find the element(s). Use null if element is to be found in the top document</param>
        public OnPageChecker TextExistsOnPage(string textToFind, string overrideTargetFrameName = null)
        {
            bool textExistsOnPage;

            if (string.IsNullOrWhiteSpace(overrideTargetFrameName))
            {
                textExistsOnPage = _driver.PageSource.Contains(textToFind);
            }
            else
            {
                textExistsOnPage = _driver.SwitchTo().Frame(overrideTargetFrameName).PageSource.Contains(textToFind);
                _driver.SwitchTo().DefaultContent();
            }

            if (!textExistsOnPage)
            {
                _stringBuilder.AppendLine(
                    $"Expected to find text '{textToFind}' on page {_pageName}, but was not found.");
            }

            _allValid = textExistsOnPage && _allValid;
            return this;
        }

        /// <summary>
        /// Checks that expected page is displayed using the given <paramref name="function"/> as the checking logic
        /// </summary>
        /// <param name="function">A function to determine whether expected page is displayed</param>
        /// <exception cref="Exception">If delegate callback throws an exception</exception>
        public OnPageChecker CustomCheckOnPage(Func<bool> function)
        {
            _allValid = function() && _allValid;
            return this;
        }

        /// <summary>
        /// Uses the given value to indicate whether the page is the expected one
        /// </summary>
        /// <param name="isValid"></param>
        public OnPageChecker CustomCheckOnPage(bool isValid)
        {
            _allValid = isValid && _allValid;
            return this;
        }

        /// <summary>
        /// Checks that at least one of the elements whose locating mechanisms are specified in <param name="bys"/> exists on the page
        /// </summary>
        /// <param name="bys">List of element locating mechanisms to find elements on the page</param>
        /// <returns></returns>
        public OnPageChecker AnyElementExistsOnPage(params By[] bys)
        {
            return AnyElementExistsOnPage(null, bys);
        }

        /// <summary>
        /// Checks that at least one of the elements whose locating mechanisms are specified in <param name="bys"/> exists on the page
        /// </summary>
        /// <param name="bys">List of element locating mechanisms to find elements on the page</param>
        /// <param name="overrideTargetFrameName">Id or name of the frame in which to find the element(s). Use null if element is to be found in the top document</param>
        /// /// <returns></returns>
        public OnPageChecker AnyElementExistsOnPage(string overrideTargetFrameName, params By[] bys)
        {
            var elementExistsOnPage = false;

            var byList = bys as IList<By> ?? bys.ToList();
            try
            {
                byList.ForEach(@by => elementExistsOnPage |= _LocalDoesElementExistOnPage(@by, overrideTargetFrameName));

                if (!string.IsNullOrWhiteSpace(overrideTargetFrameName))
                {
                    _driver.SwitchTo().DefaultContent();
                }
            }
            catch (NotFoundException)
            {
                elementExistsOnPage = false;
            }

            if (!elementExistsOnPage)
            {
                _stringBuilder.AppendLine(
                    $"Expected to find at least one of the following elements on page {_pageName}, but none was found: '{string.Join(" or ", byList)}'.");
            }

            _allValid = elementExistsOnPage && _allValid;
            return this;
        }

        /// <summary>
        /// Checks that all of the elements whose locating mechanisms are specified in <param name="bys"/> exist on the page
        /// </summary>
        /// <param name="bys">List of element locating mechanisms to find elements on the page</param>
        /// <returns></returns>
        public OnPageChecker AllElementsExistOnPage(params By[] bys)
        {
            return AllElementsExistOnPage(null, bys);
        }

        /// <summary>
        /// Checks that all of the elements whose locating mechanisms are specified in <param name="bys"/> exist on the page
        /// </summary>
        /// <param name="bys">List of element locating mechanisms to find elements on the page</param>
        /// <param name="overrideTargetFrameName">Id or name of the frame in which to find the element(s). Use null if element is to be found in the top document</param>
        public OnPageChecker AllElementsExistOnPage(string overrideTargetFrameName, params By[] bys)
        {
            var allElementsExistOnPage = true;
            var byList = bys as IList<By> ?? bys.ToList();

            foreach (var @by in byList)
            {
                allElementsExistOnPage &= _LocalDoesElementExistOnPage(@by, overrideTargetFrameName);
                if (!string.IsNullOrWhiteSpace(overrideTargetFrameName))
                {
                    _driver.SwitchTo().DefaultContent();
                }
            }
            
            if (!allElementsExistOnPage)
            {
                _stringBuilder.AppendLine(
                    $"Expected to find all of the following elements on page {_pageName}, but at least one was not found: '{string.Join(" or ", byList)}'.");
            }

            _allValid = allElementsExistOnPage && _allValid;
            return this;
        }

        /// <summary>
        /// Checks that an element with given HTML id exists on the page
        /// </summary>
        /// <param name="elementId">HTML id of the element to check the existance of</param>
        /// <param name="overrideTargetFrameName">Id or name of the frame in which to find the element(s). Use null if element is to be found in the top document</param>
        public OnPageChecker ElementExistsOnPage(string elementId, string overrideTargetFrameName = null)
        {
            bool elementExistsOnPage;

            try
            {
                elementExistsOnPage = _LocalDoesElementExistOnPage(By.Id(elementId), overrideTargetFrameName);

                if (!string.IsNullOrWhiteSpace(overrideTargetFrameName))
                {
                    _driver.SwitchTo().DefaultContent();
                }
            }
            catch (NotFoundException)
            {
                elementExistsOnPage = false;
            }

            if (!elementExistsOnPage)
            {
                _stringBuilder.AppendLine(
                    $"Expected to find element with id '{elementId}' on page {_pageName}, but was not found.");
            }

            _allValid = elementExistsOnPage && _allValid;
            return this;
        }

        /// <summary>
        /// Checks that an element with given locating mechanism exists on the page
        /// </summary>
        /// <param name="by">Locating mechanism for the element to check the existence of on the page</param>
        /// <param name="overrideTargetFrameName">Id or name of the frame in which to find the element(s). Use null if element is to be found in the top document</param>
        public OnPageChecker ElementExistsOnPage(By by, string overrideTargetFrameName = null)
        {
            bool elementExistsOnPage;

            try
            {
                elementExistsOnPage = _LocalDoesElementExistOnPage(by, overrideTargetFrameName);

                if (!string.IsNullOrWhiteSpace(overrideTargetFrameName))
                {
                    _driver.SwitchTo().DefaultContent();
                }
            }
            catch (NotFoundException)
            {
                elementExistsOnPage = false;
            }

            if (!elementExistsOnPage)
            {
                _stringBuilder.AppendLine(
                    $"Expected to find element with id '{@by}' on page {_pageName}, but was not found.");
            }

            _allValid = elementExistsOnPage && _allValid;
            return this;
        }

        private bool _LocalDoesElementExistOnPage(By @by, string frame = null)
        {
            bool doesExist;
            if (frame != null)
            {
                _driver.SwitchTo().DefaultContent();
                _driver.SwitchTo().Frame(frame);
                doesExist = _driver.FindElementSafe(@by) != null;
            }
            else
            {
                doesExist = _driver.FindElementSafe(@by) != null;
            }
            return doesExist;
        }
    }
}
