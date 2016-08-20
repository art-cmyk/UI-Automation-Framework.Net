using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Keys = OpenQA.Selenium.Keys;

namespace Ravitej.Automation.Common.PageObjects.Interactables.Selenium
{
    public abstract partial class Interactable
    {
        /// <summary>
        /// Searches for an element using specified location strategy and returns first matching element or throws NotFoundException.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Description of the element to use in logs and exception messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns>The first <see cref="T:OpenQA.Selenium.IWebElement"/> matching the criteria.</returns>
        /// <exception cref="NotFoundException">If no element matches the criteria.</exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected virtual IWebElement GetElementOrThrow(By @by, string elementDescription, IWebElement frame)
        {
            SwitchToDefaultContent();
            Driver.SwitchTo().Frame(frame);
            var element = Driver.FindElementOrThrow(@by, ConstructNotFoundErrorMessage(elementDescription));
            return element;
        }

        /// <summary>
        /// Searches for an element within the given context using specified location strategy and returns it if found.
        /// </summary>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception message.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns>IWebElement object of the discovered element</returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected IWebElement GetElementOrThrow(ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            SwitchToDefaultContent();
            Driver.SwitchTo().Frame(frame);
            var element = context.FindElementOrThrow(@by, ConstructNotFoundErrorMessage(elementDescription));
            return element;
        }

        /// <summary>
        /// Searches for all elements using specified location strategy and returns them if found.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Elements description to use in exception message.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns>Collection of IWebElement objects of the discovered elements</returns>
        /// <exception cref="NotFoundException">Thrown when no elements could be found using the given location strategy. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected IEnumerable<IWebElement> GetElementsOrThrow(By @by, string elementDescription, IWebElement frame)
        {
            SwitchToDefaultContent();
            Driver.SwitchTo().Frame(frame);
            var elements = Driver.FindElementsOrThrow(@by, ConstructNotFoundErrorMessage(elementDescription));
            return elements;
        }

        /// <summary>
        /// Searches for all elements within given context using specified location strategy and returns them if found.
        /// </summary>
        /// <param name="context">Context within which to search for the elements.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Elements description to use in exception message.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns>Collection of IWebElement objects of the discovered elements</returns>
        /// <exception cref="NotFoundException">Thrown when no elements could be found in the given context using the given location strategy.</exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected IEnumerable<IWebElement> GetElementsOrThrow(ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            SwitchToDefaultContent();
            Driver.SwitchTo().Frame(frame);
            var elements = context.FindElementsOrThrow(@by, ConstructNotFoundErrorMessage(elementDescription));
            return elements;
        }

        /// <summary>
        /// Gets a value indicating whether element is present in DOM.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns>True if element is found, otherwise false.</returns>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected bool IsElementPresent(By @by, string elementDescription, IWebElement frame)
        {
            try
            {
                GetElementOrThrow(@by, elementDescription, frame);
                return true;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether element is present in DOM within the given context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected bool IsElementPresent(ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            try
            {
                GetElementOrThrow(context, @by, elementDescription, frame);
                return true;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }
        /// <summary>
        /// Gets a value indicating whether element is present in DOM and whether it is displayed.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns>True if the element is found AND displayed, otherwise false</returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected bool IsElementDisplayed(By @by, string elementDescription, IWebElement frame)
        {
            try
            {
                var elementDisplayed = GetElementOrThrow(@by, elementDescription, frame).Displayed;
                WriteInfoLogEntry(
                    elementDisplayed
                        ? "{0} ({1}) found and is displayed"
                        : "{0} ({1}) found, but is not displayed", elementDescription, @by.ToString());
                return elementDisplayed;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }
        /// <summary>
        /// Gets a value indicating whether element is present in DOM within given context and whether it is displayed.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns>True if the element is found AND displayed, otherwise false</returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected bool IsElementDisplayed(ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            try
            {
                var elementDisplayed = GetElementOrThrow(context, @by, elementDescription, frame).Displayed;
                WriteInfoLogEntry(
                    elementDisplayed
                        ? "{0} ({1}) found within given context and is displayed"
                        : "{0} ({1}) found within given context, but is not displayed", elementDescription, @by.ToString());
                return elementDisplayed;
            }
            catch (NotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Finds checkbox/radio button element using specified location strategy, gets its ticked status and logs the action.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected bool IsTicked(By by, string elementDescription, IWebElement frame)
        {
            var checkbox = GetElementOrThrow(by, elementDescription, frame);
            var isTicked = checkbox.IsTicked();
            WriteInfoLogEntry(ConstructDataFetchedMessage(isTicked.ToString(), elementDescription));
            return isTicked;
        }

        /// <summary>
        /// Finds checkbox/radio button element using specified location strategy within given context, gets its ticked status and logs the action.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected bool IsTicked(ISearchContext context, By by, string elementDescription, IWebElement frame)
        {
            var checkbox = GetElementOrThrow(context, by, elementDescription, frame);
            var isTicked = checkbox.IsTicked();
            WriteInfoLogEntry(ConstructDataFetchedMessage(isTicked.ToString(), elementDescription));
            return isTicked;
        }

        /// <summary>
        /// Finds checkbox element using specified location strategy, ticks if it is not already ticked and logs the action.
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        protected void Tick(WebElementType webElementType, By @by, string elementDescription, IWebElement frame)
        {
            var checkbox = GetElementOrThrow(@by, elementDescription, frame);
            switch (webElementType)
            {
                case WebElementType.HtmlCheckbox:
                    {
                        checkbox.Tick();
                        SwitchToDefaultContent();
                        break;
                    }
                case WebElementType.JQueryCheckbox:
                    {
                        var accompanyingLabel = _LabelAccompanyingCheckbox(@by, elementDescription, frame);
                        checkbox.Tick(accompanyingLabel);
                        SwitchToDefaultContent();
                        break;
                    }
            }
            WriteInfoLogEntry(ConstructCheckboxClickedMessage(elementDescription, true));
        }

        private IWebElement _LabelAccompanyingCheckbox(By @by, string elementDescription, IWebElement frame)
        {
            IWebElement accompanyingLabel;
            var checkboxId = GetId(@by, elementDescription);
            var accompanyingLabelDesc = _associatedElementDesc(elementDescription, "label");
            try
            {
                accompanyingLabel = GetElementOrThrow(By.Id(string.Concat(checkboxId, "_label")), accompanyingLabelDesc, frame);
            }
            catch (NotFoundException)
            {
                accompanyingLabel = GetElementOrThrow(By.CssSelector($"label[for='{checkboxId}']"),
                    accompanyingLabelDesc, frame);
            }
            return accompanyingLabel;
        }

        /// <summary>
        /// Finds checkbox element using specified location strategy within given context, ticks if it is not already ticked and logs the action.
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected void Tick(WebElementType webElementType, ISearchContext context, By @by, string elementDescription,
            IWebElement frame)
        {
            var checkbox = GetElementOrThrow(context, @by, elementDescription, frame);
            switch (webElementType)
            {
                case WebElementType.HtmlCheckbox:
                    {
                        checkbox.Tick();
                        SwitchToDefaultContent();
                        break;
                    }
                case WebElementType.JQueryCheckbox:
                    {
                        var accompanyingLabel = _LabelAccompanyingCheckbox(@by, elementDescription, frame);
                        checkbox.Tick(accompanyingLabel);
                        SwitchToDefaultContent();
                        break;
                    }
            }
            WriteInfoLogEntry(ConstructCheckboxClickedMessage(elementDescription, true));
        }

        /// <summary>
        /// Finds checkbox element using specified location strategy, unticks if it is already ticked and logs the action.
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        protected void Untick(WebElementType webElementType, By @by, string elementDescription, IWebElement frame)
        {
            var checkbox = GetElementOrThrow(@by, elementDescription, frame);
            switch (webElementType)
            {
                case WebElementType.HtmlCheckbox:
                    {
                        checkbox.Untick();
                        SwitchToDefaultContent();
                        break;
                    }
                case WebElementType.JQueryCheckbox:
                    {
                        var accompanyingLabel = _LabelAccompanyingCheckbox(@by, elementDescription, frame);
                        checkbox.Untick(accompanyingLabel);
                        SwitchToDefaultContent();
                        break;
                    }
            }
            WriteInfoLogEntry(ConstructCheckboxClickedMessage(elementDescription, false));
        }

        /// <summary>
        /// Finds checkbox element using specified location strategy with given context, unticks if it is already ticked and logs the action.
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        protected void Untick(WebElementType webElementType, ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            var checkbox = GetElementOrThrow(context, @by, elementDescription, frame);
            switch (webElementType)
            {
                case WebElementType.HtmlCheckbox:
                    {
                        checkbox.Untick();
                        SwitchToDefaultContent();
                        break;
                    }
                case WebElementType.JQueryCheckbox:
                    {
                        var accompanyingLabel = _LabelAccompanyingCheckbox(@by, elementDescription, frame);
                        checkbox.Untick(accompanyingLabel);
                        SwitchToDefaultContent();
                        break;
                    }
            }
            WriteInfoLogEntry(ConstructCheckboxClickedMessage(elementDescription, false));
        }

        /// <summary>
        /// Finds element using specified location strategy, clicks on it and logs the action. 
        /// If the element is not in view, it is scrolled into view and then clicked.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected void Click(By @by, string elementDescription, IWebElement frame)
        {
            var element = GetElementOrThrow(@by, elementDescription, frame);
            TryClickOrScroll(element);
            //SwitchToDefaultContent(); //commenting out to avoid UnhandledAlertException if an alert comes up due to clicking
            WriteInfoLogEntry(ConstructClickedMessage(elementDescription));
        }

        /// <summary>
        /// Finds element using specified location strategy within given context, clicks on it and logs the action. 
        /// If the element is not in view, it is scrolled into view and then clicked.
        /// </summary>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected void Click(ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            var element = GetElementOrThrow(context, @by, elementDescription, frame);
            TryClickOrScroll(element);
            //SwitchToDefaultContent(); //commenting out to avoid UnhandledAlertException if an alert comes up due to clicking
            WriteInfoLogEntry(ConstructClickedMessage(elementDescription));
        }

        /// <summary>
        /// Gets the inner text of element (other than textox or textrea - returns empty string) without any leading or trailing spaces and logs the action.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns>Returns a string containing the text of the element.</returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected string GetText(By @by, string elementDescription, IWebElement frame)
        {
            var text = GetElementOrThrow(@by, elementDescription, frame).GetText();
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }

        /// <summary>
        /// Gets the inner text of element (other than textox or textrea - returns empty string) without any leading or trailing spaces and logs the action.
        /// </summary>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected string GetText(ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            var text = GetElementOrThrow(context, @by, elementDescription, frame).GetText();
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }
        /// <summary>
        /// Clears the text of element (only editable elements - textox or textrea) and logs the action.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">If no element matches the criteria.</exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected void ClearText(By @by, string elementDescription, IWebElement frame)
        {
            var element = GetElementOrThrow(@by, elementDescription, frame);
            var text = element.GetText();
            element.Clear();
            WriteInfoLogEntry(ConstructDataClearedMessage(text, elementDescription));
        }
        /// <summary>
        /// Clears the text of element (only editable elements - textox or textrea) and logs the action.
        /// </summary>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">If no element matches the criteria.</exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected void ClearText(ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            var element = GetElementOrThrow(context, @by, elementDescription, frame);
            var text = element.GetText();
            element.Clear();
            WriteInfoLogEntry(ConstructDataClearedMessage(text, elementDescription));
        }
        /// <summary>
        /// Gets the value of 'value' attribute of a textbox or textarea without any leading or trailing spaces
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected string GetValue(By @by, string elementDescription, IWebElement frame)
        {
            var text = GetElementOrThrow(@by, elementDescription, frame).GetValue();
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }

        /// <summary>
        /// Gets the value of 'value' attribute of a textbox or textarea without any leading or trailing spaces
        /// </summary>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected string GetValue(ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            var text = GetElementOrThrow(context, @by, elementDescription, frame).GetValue();
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }

        /// <summary>
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="value"></param>
        /// <param name="blurAfterEnter"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        protected void EnterText(By @by, string elementDescription, string value, bool blurAfterEnter,
            IWebElement frame)
        {
            GetElementOrThrow(@by, elementDescription, frame).EnterValue(value);
            if (blurAfterEnter)
            {
                _PerformJavaScriptBlur(@by, elementDescription);
            }
            WriteInfoLogEntry(ConstructDataEnteredMessage(value, elementDescription));
        }

        /// <summary>
        /// </summary>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="value"></param>
        /// <param name="blurAfterEnter"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        protected void EnterText(ISearchContext context, By @by, string elementDescription, string value,
            bool blurAfterEnter, IWebElement frame)
        {
            GetElementOrThrow(context, @by, elementDescription, frame).EnterValue(value);
            if (blurAfterEnter)
            {
                _PerformJavaScriptBlur(@by, elementDescription);
            }
            WriteInfoLogEntry(ConstructDataEnteredMessage(value, elementDescription));
        }

        /// <summary>
        /// Sets the given text into the clipboard, then finds element using specified location strategy and pastes into it.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="textToPaste">Text to paste into the element.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">If no element matches the criteria.</exception>
        /// <exception cref="NoSuchFrameException">If the frame with Id or name specified in frame cannot be found.</exception>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected void PasteText(By @by, string elementDescription, string textToPaste, IWebElement frame)
        {
            _SetClipboardText(textToPaste);
            GetElementOrThrow(@by, elementDescription, frame).PasteData();
            WriteInfoLogEntry(ConstructDataPastedMessage(textToPaste, elementDescription));
        }

        /// <summary>
        /// Sets the given text into the clipboard, then finds element using specified location strategy and pastes into it.
        /// </summary>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription">Element description to use in exception and log messages.</param>
        /// <param name="textToPaste">Text to paste into the element.</param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NoSuchFrameException">If the frame with Id or name specified in frame cannot be found.</exception>
        /// <exception cref="NotFoundException">If no element matches the criteria.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        protected void PasteText(ISearchContext context, By @by, string elementDescription, string textToPaste, IWebElement frame)
        {
            _SetClipboardText(textToPaste);
            GetElementOrThrow(context, @by, elementDescription, frame).PasteData();
            WriteInfoLogEntry(ConstructDataPastedMessage(textToPaste, elementDescription));
        }
        /// <summary>
        /// Finds autocomplete textbox which looks like combobox and selects the given value from it and logs the action
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="value"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame cannot be found.</exception>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected void SelectText(By @by, string elementDescription, string value, IWebElement frame)
        {
            var element = GetElementOrThrow(@by, elementDescription, frame);
            element.EnterValue(value);
            element.SendKeys(Keys.ArrowDown + Keys.Enter);
            WriteInfoLogEntry(ConstructDataSelectedMessage(value, elementDescription));
        }

        /// <summary>
        /// Finds autocomplete textbox which looks like combobox and selects the given value from it and logs the action
        /// </summary>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="value"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame cannot be found.</exception>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected void SelectText(ISearchContext context, By @by, string elementDescription, string value, IWebElement frame)
        {
            var element = GetElementOrThrow(context, @by, elementDescription, frame);
            element.EnterValue(value);
            element.SendKeys(Keys.ArrowDown + Keys.Enter);
            WriteInfoLogEntry(ConstructDataSelectedMessage(value, elementDescription));
        }

        /// <summary>
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="value"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="UnexpectedTagNameException">Thrown when the element wrapped is not a &lt;select&gt; element.</exception>
        /// <exception cref="NoSuchElementException">Thrown if there is no element with the given text present.</exception>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected void SelectByText(WebElementType webElementType, By @by, string elementDescription, string value, IWebElement frame)
        {
            switch (webElementType)
            {
                case WebElementType.HtmlCombo:
                    {
                        var element = GetElementOrThrow(@by, elementDescription, frame);
                        new SelectElement(element).SelectByText(value);
                        break;
                    }
                case WebElementType.JQueryCombo:
                    {
                        var element2 = GetElementOrThrow(By.Id(string.Concat(GetId(@by, elementDescription), "_input")), _associatedElementDesc(elementDescription, "input"), frame);
                        element2.EnterValue(value);
                        element2.SendKeys(Keys.ArrowDown + Keys.Enter);
                        break;
                    }
            }
            WriteInfoLogEntry(ConstructDataSelectedMessage(value, elementDescription));
        }

        /// <summary>
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="value"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="UnexpectedTagNameException">Thrown when the element wrapped is not a &lt;select&gt; element.</exception>
        /// <exception cref="NoSuchElementException">Thrown if there is no element with the given text present.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        protected void SelectByText(WebElementType webElementType, ISearchContext context, By @by, string elementDescription, string value, IWebElement frame)
        {
            switch (webElementType)
            {
                case WebElementType.HtmlCombo:
                    {
                        var element = GetElementOrThrow(context, @by, elementDescription, frame);
                        new SelectElement(element).SelectByText(value);
                        break;
                    }
                case WebElementType.JQueryCombo:
                    {
                        var element2 = GetElementOrThrow(context, By.Id(string.Concat(GetId(@by, elementDescription), "_input")), _associatedElementDesc(elementDescription, "input"), frame);
                        element2.EnterValue(value);
                        element2.SendKeys(Keys.ArrowDown + Keys.Enter);
                        break;
                    }
            }
            WriteInfoLogEntry(ConstructDataSelectedMessage(value, elementDescription));
        }

        /// <summary>
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected string GetSelectedText(WebElementType webElementType, By @by, string elementDescription, IWebElement frame)
        {
            var element = GetElementOrThrow(@by, elementDescription, frame);
            var text = element.GetSelectedText(webElementType);
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }

        /// <summary>
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected string GetSelectedText(WebElementType webElementType, ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            var element = GetElementOrThrow(context, @by, elementDescription, frame);
            var text = element.GetSelectedText(webElementType);
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }

        /// <summary>
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected IEnumerable<string> GetAllOptionsText(WebElementType webElementType, By @by, string elementDescription, IWebElement frame)
        {
            var element = GetElementOrThrow(@by, elementDescription, frame);
            var text = element.GetAllOptionsText(webElementType).ToList();
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }

        /// <summary>
        /// </summary>
        /// <param name="webElementType"></param>
        /// <param name="context">Context within which to search for the element.</param>
        /// <param name="by">The locating mechanism to use.</param>
        /// <param name="elementDescription"></param>
        /// <param name="frame">Previously located <see cref="T:OpenQA.Selenium.IWebElement"/> of the frame in which to find the element.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        /// <exception cref="NoSuchFrameException">If the frame specified in <paramref name="frame"/> cannot be found.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        protected IEnumerable<string> GetAllOptionsText(WebElementType webElementType, ISearchContext context, By @by, string elementDescription, IWebElement frame)
        {
            var element = GetElementOrThrow(context, @by, elementDescription, frame);
            var text = element.GetAllOptionsText(webElementType).ToList();
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }
    }
}
