using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace OpenQA.Selenium
{
    /// <summary>
    /// Extension methods for the WebElement
    /// </summary>
    public static class WebElementExtensions
    {
        private static readonly string Type = typeof(WebElementExtensions).ToString();

        /// <summary>
        /// Get a rectangle defining the size of the element
        /// </summary>
        /// <param name="oElement"></param>
        /// <returns></returns>
        public static Rectangle GetRectangle(this IWebElement oElement)
        {
            return new Rectangle(oElement.Location, oElement.Size);
        }

        /// <summary>
        /// Indicates if the element has a given CSS class or not
        /// </summary>
        /// <param name="oElement"></param>
        /// <param name="sClassName"></param>
        /// <returns></returns>
        public static bool HasClass(this IWebElement oElement, string sClassName)
        {
            return (oElement.GetAttribute("class") ?? string.Empty).Split(' ').Contains(sClassName);
        }

        /// <summary>
        /// Indicates if the element is displayed in the UI
        /// </summary>
        /// <param name="oElement"></param>
        /// <returns></returns>
        public static bool IsDisplayed(this IWebElement oElement)
        {
            return oElement.GetCssValue("display") != "none";
        }

        /// <summary>
        /// Gets all attributes of the element and their values
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns>A dictionary with attributes in keys and their values in values</returns>
        public static Dictionary<string, object> GetAllAttributes(this IWebElement webElement)
        {
            var driver = ((IWrapsDriver)webElement).WrappedDriver;
            var jsExecutor = (IJavaScriptExecutor)driver;
            const string javascript =
                "var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;";
            return jsExecutor.ExecuteScript(javascript, webElement) as Dictionary<string, object>;
        }
        /// <summary>
        /// Determines if the element has a given attribute
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="attribute"></param>
        /// <returns>True if the element has the given attribute and False if not</returns>
        public static bool HasAttribute(this IWebElement webElement, string attribute)
        {
            var attributes = webElement.GetAllAttributes();
            return attributes.ContainsKey(attribute);
        }
        /// <summary>
        /// Gets the 'value' attribute of textbox or text area
        /// </summary>
        /// <param name="oElement"></param>
        /// <returns>A whitespace trimmed string containing value in textbox or text area</returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public static string GetValue(this IWebElement oElement)
        {
            return oElement.GetAttribute("value").Trim();
        }
        /// <summary>
        /// Gets the inner text of this element without any leading or trailing spaces
        /// </summary>
        /// <param name="element"></param>
        /// <returns>A whitespace trimmed string containing text of the element</returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public static string GetText(this IWebElement element)
        {
            return element.Text.Trim();
        }
        /// <summary>
        /// Clears existing value and enters value passed in into textbox or text area
        /// </summary>
        /// <param name="oElement"></param>
        /// <param name="value">Value to be entered into textbox or text area</param>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public static void EnterValueMobile(this IWebElement oElement, string value)
        {
            oElement.Clear();
            oElement.SendKeys(value);
        }
        /// <summary>
        /// Clears existing value and enters value passed in into textbox or text area
        /// </summary>
        /// <param name="oElement"></param>
        /// <param name="value">Value to be entered into textbox or text area</param>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public static void EnterValue(this IWebElement oElement, string value)
        {
            oElement.SelectAll().SendKeys(value);
        }
        /// <summary>
        /// Pastes current data in the system clipboard into the element. Only works on Windows environments
        /// </summary>
        /// <param name="element">Element into which to paste data</param>
        /// <exception cref="InvalidElementStateException">Thrown when the target element is not enabled.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public static void PasteData(this IWebElement element)
        {
            element.SelectAll().SendKeys(Keys.Control + 'v');
        }
        /// <summary>
        /// Gets a value indicating whether or not the checkbox is ticked.
        /// This operation only applies to checkboxes and radio buttons.
        /// </summary>
        /// <param name="inputElement"></param>
        /// <returns>True if the checkbox/radio button is ticked/selected and false otherwise.</returns>
        public static bool IsTicked(this IWebElement inputElement)
        {
            return inputElement.Selected;
        }
        /// <summary>
        /// Ticks JQuery checkbox if not already ticked
        /// </summary>
        /// <param name="inputElement">Hidden input that represents the checkbox</param>
        /// <param name="accompanyingElement">Element accompanying the input that interacts on the UI</param>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        public static void Tick(this IWebElement inputElement, IWebElement accompanyingElement)
        {
            if (inputElement.Selected == false)
            {
                accompanyingElement.Click();
            }
        }
        /// <summary>
        /// Unticks JQuery checkbox if already ticked
        /// </summary>
        /// <param name="inputElement">Hidden input that represents the checkbox</param>
        /// <param name="accompanyingElement">Element accompanying the input that interacts on the UI</param>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public static void Untick(this IWebElement inputElement, IWebElement accompanyingElement)
        {
            if (inputElement.Selected)
            {
                accompanyingElement.Click();
            }
        }
        /// <summary>
        /// Ticks HTML checkbox if not already ticked
        /// </summary>
        /// <param name="inputElement">Input that represents the checkbox</param>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public static void Tick(this IWebElement inputElement)
        {
            if (inputElement.Selected == false)
            {
                inputElement.Click();
            }
        }

        /// <summary>
        /// Unticks HTML checkbox if already ticked
        /// </summary>
        /// <param name="inputElement">Input that represents the checkbox</param>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public static void Untick(this IWebElement inputElement)
        {
            if (inputElement.Selected)
            {
                inputElement.Click();
            }
        }
        /// <summary>
        /// Performs a drag and drop operation from source element to target element
        /// </summary>
        /// <param name="sourceWebElement">The element on which the drag operation is started</param>
        /// <param name="targetWebElement">The element on which the drop is performed</param>
        public static void DragAndDrop(this IWebElement sourceWebElement, IWebElement targetWebElement)
        {
            var driver = ((IWrapsDriver)sourceWebElement).WrappedDriver;
            var actions = new Actions(driver);
            actions.DragAndDrop(sourceWebElement, targetWebElement).Build().Perform();

            //Alternative code if the above code doesn't work.
            //actions.ClickAndHold(sourceWebElement)
            //   .MoveToElement(targetWebElement)
            //   .Release(targetWebElement) //this line may not be needed. check without it first.
            //   .Build()
            //   .Perform();
        }
        /// <summary>
        /// Performs a drag and drop operation from source element to a specified offset of the source element
        /// </summary>
        /// <param name="sourceWebElement">The element on which the drag operation is started</param>
        /// <param name="offsetX">The horizontal offset to which to move the mouse</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse</param>
        public static void DragAndDrop(this IWebElement sourceWebElement, int offsetX, int offsetY)
        {
            var driver = ((IWrapsDriver)sourceWebElement).WrappedDriver;
            var actions = new Actions(driver);
            actions.DragAndDropToOffset(sourceWebElement, offsetX, offsetY).Build().Perform();
        }
        /// <summary>
        /// Performs a drag and drop operation from source element to a specified offset of the top-left corner of target element
        /// </summary>
        /// <param name="sourceWebElement">The element on which the drag operation is started</param>
        /// <param name="targetWebElement">The element on which the drop is performed</param>
        /// <param name="offsetX">The horizontal offset to which to move the mouse</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse</param>
        public static void DragAndDrop(this IWebElement sourceWebElement, IWebElement targetWebElement, int offsetX, int offsetY)
        {
            var driver = ((IWrapsDriver)sourceWebElement).WrappedDriver;
            var actions = new Actions(driver);

            actions.ClickAndHold(sourceWebElement)
                .MoveToElement(targetWebElement, offsetX, offsetY)
                .Release(targetWebElement) //this line may not be needed. check without it first.
                .Build()
                .Perform();
        }
        /// <summary>
        /// Moves mouse pointer to the centre of the element 
        /// </summary>
        /// <param name="oElement"></param>
        public static void MoveMouseToCentre(this IWebElement oElement)
        {
            var oDriver = ((IWrapsDriver)oElement).WrappedDriver;
            ((RemoteWebDriver)oDriver).Mouse.MouseMove(((RemoteWebElement)oElement).Coordinates, oElement.Size.Width / 2, oElement.Size.Height / 2);
        }
        /// <summary>
        /// Moves mouse pointer to the specified offset of the element's top-left corner and clicks
        /// </summary>
        /// <param name="element"></param>
        /// <param name="offsetX">The horizontal offset to which to move the mouse</param>
        /// <param name="offsetY">The vertical offset to which to move the mouse</param>
        public static void MoveMouseAndClick(this IWebElement element, int offsetX, int offsetY)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var actions = new Actions(driver);
            actions
                .MoveToElement(element, offsetX, offsetY)
                .Click()
                .Build()
                .Perform();
        }
        /// <summary>
        /// Finds all elements within the current context using the given mechanism
        /// </summary>
        /// <param name="searchContext"></param>
        /// <param name="by"></param>
        /// <returns>Returns a collection of IWebElement if found or null if nothing found</returns>
        public static IEnumerable<IWebElement> FindElementsSafe(this ISearchContext searchContext, By @by)
        {
            var elements = searchContext.FindElements(@by);
            return elements.Any() ? elements : null;
        }
        /// <summary>
        /// Finds all elements within the current context using the given mechanism and matching the given condition
        /// </summary>
        /// <param name="searchContext"></param>
        /// <param name="by"></param>
        /// <param name="fnCondition"></param>
        /// <returns>Returns a collection of IWebElement if found and condition satisfied or null if nothing found or condition not satisfied</returns>
        public static IEnumerable<IWebElement> FindElementsSafe(this ISearchContext searchContext, By @by, Func<IWebElement, bool> fnCondition)
        {
            var elements = searchContext.FindElements(@by).Where(fnCondition).ToList();
            return elements.Any() ? elements : null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchContext"></param>
        /// <param name="by"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        public static IEnumerable<IWebElement> FindElementsOrThrow(this ISearchContext searchContext, By @by, string exceptionMessage)
        {
            var elements = searchContext.FindElementsSafe(@by);
            if (elements == null)
            {
                var message = $"{exceptionMessage}. Elements locator: {@by}";
                LogX.Warning.Category(Type).Write(message);
                throw new NotFoundException(message);
            }
            LogX.Info.Category(Type).Write("Elements found: {0}", @by.ToString());
            return elements;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchContext"></param>
        /// <param name="by"></param>
        /// <param name="fnCondition"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found.</exception>
        public static IEnumerable<IWebElement> FindElementsOrThrow(this ISearchContext searchContext, By @by, Func<IWebElement, bool> fnCondition, string exceptionMessage)
        {
            var elements = searchContext.FindElementsSafe(@by, fnCondition);
            if (elements == null)
            {
                var message = $"{exceptionMessage}. Elements locator: {@by}";
                LogX.Warning.Category(Type).Write(message);
                throw new NotFoundException(message);
            }
            LogX.Info.Category(Type).Write("Elements found: {0}", @by.ToString());
            return elements;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oElement"></param>
        /// <param name="oBy"></param>
        /// <returns></returns>
        public static IWebElement FindElementSafe(this ISearchContext oElement, By oBy)
        {
            return oElement.FindElements(oBy).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oElement"></param>
        /// <param name="oBy"></param>
        /// <param name="fnCondition"></param>
        /// <returns></returns>
        public static IWebElement FindElementSafe(this ISearchContext oElement, By oBy, Func<IWebElement, bool> fnCondition)
        {
            return oElement.FindElements(oBy).FirstOrDefault(fnCondition);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oDriver"></param>
        /// <param name="oBy"></param>
        /// <param name="sNotFoundExceptionText"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        public static IWebElement FindElementOrThrow(this ISearchContext oDriver, By oBy, string sNotFoundExceptionText)
        {
            IWebElement oElement = oDriver.FindElementSafe(oBy);
            if (oElement == null)
            {
                var message = $"{sNotFoundExceptionText}. Element locator: {oBy}";
                LogX.Warning.Category(Type).Write(message);
                throw new NotFoundException(message);
            }
            LogX.Info.Category(Type).Write("Element found: {0}", oBy.ToString());
            return oElement;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oDriver"></param>
        /// <param name="oBy"></param>
        /// <param name="fnCondition"></param>
        /// <param name="sNotFoundExceptionText"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Thrown when element cannot be found. </exception>
        public static IWebElement FindElementOrThrow(this ISearchContext oDriver, By oBy, Func<IWebElement, bool> fnCondition, string sNotFoundExceptionText)
        {
            IWebElement oElement = oDriver.FindElementSafe(oBy, fnCondition);
            if (oElement == null)
            {
                var message = $"{sNotFoundExceptionText}. Element locator: {oBy}";
                LogX.Warning.Category(Type).Write(message);
                throw new NotFoundException(message);
            }
            LogX.Info.Category(Type).Write("Element found: {0}", oBy.ToString());
            return oElement;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oElement"></param>
        /// <returns></returns>
        public static IWebElement SelectAll(this IWebElement oElement)
        {
            oElement.SendKeys(Keys.Control + "a");
            return oElement;
        }
        /// <summary>
        /// Performs the <paramref name="action"/> which causes new window to open.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="action">Action to perform which causes new window to open.</param>
        /// <param name="oldWindowHandle">Window handle for the existing window (in order to be able to switch back to it).</param>
        /// <param name="newWindowHandle">Window handle for the new window (in order to be able to switch to it and do something).</param>
        public static void OpenNewWindow(this IWebDriver driver, Action action, out string oldWindowHandle, out string newWindowHandle)
        {
            oldWindowHandle = driver.CurrentWindowHandle;
            ReadOnlyCollection<string> enOldWindowHandles = driver.WindowHandles;
            action();
            Thread.Sleep(2000);
            ReadOnlyCollection<string> enNewWindowHandles = driver.WindowHandles;
            newWindowHandle = enNewWindowHandles.Except(enOldWindowHandles).FirstOrDefault();
        }
        /// <summary>
        /// Performs the <paramref name="action"/> which causes new window to open.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="action">Action to perform which causes new window to open.</param>
        /// <param name="oldWindowHandle">Window handle for the existing window (in order to be able to switch back to it).</param>
        /// <param name="newWindowHandle">Window handle for the new window (in order to be able to switch to it and do something).</param>
        public static void NewWindow(this IWebDriver driver, Action action, out string oldWindowHandle, out string newWindowHandle)
        {
            oldWindowHandle = driver.CurrentWindowHandle;
            var finder = new PopupWindowFinder(driver, new TimeSpan(0, 1, 0));
            newWindowHandle = finder.Invoke(action);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceContainer"></param>
        /// <returns></returns>
        public static IEnumerable<IWebElement> GetAllInteractableElements(this IWebElement sourceContainer)
        {
            var allItems = new List<IWebElement>();

            allItems.AddRange(sourceContainer.FindElements(By.TagName("input")));

            // remove all input type-hidden fields
            allItems.RemoveAll(s => s.GetAttribute("type") == "hidden");

            allItems.AddRange(sourceContainer.FindElements(By.TagName("select")));
            allItems.AddRange(sourceContainer.FindElements(By.TagName("button")));
            allItems.AddRange(sourceContainer.FindElements(By.TagName("a")));

            return allItems;
        }
        /// <summary>
        /// Returns the selected option text from a combobox
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="webElementType">The type of the element (Html or Jquery)</param>
        /// <returns></returns>
        public static string GetSelectedText(this IWebElement webElement, WebElementType webElementType)
        {
            var driver = ((IWrapsDriver)webElement).WrappedDriver;
            var jsExecutor = (IJavaScriptExecutor)driver;
            string text = null;
            switch (webElementType)
            {
                case WebElementType.HtmlCombo:
                    {
                        text = new SelectElement(webElement).SelectedOption.GetText();
                        break;
                    }
                case WebElementType.JQueryCombo:
                    {
                        text = (string)jsExecutor.ExecuteScript("return arguments[0].options[arguments[0].selectedIndex].text;", webElement);
                        break;
                    }
            }
            return text;
        }
        /// <summary>
        /// Returns the text of all options from a combobox
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="webElementType"></param>
        /// <returns>An IEnumerable with text of all options in the combobox</returns>
        public static IEnumerable<string> GetAllOptionsText(this IWebElement webElement, WebElementType webElementType)
        {
            var driver = ((IWrapsDriver)webElement).WrappedDriver;
            var jsExecutor = (IJavaScriptExecutor)driver;
            IEnumerable<string> text = null;

            switch (webElementType)
            {
                case WebElementType.HtmlCombo:
                    {
                        var allOptions = new SelectElement(webElement).Options;
                        text = allOptions.Select(option => option.GetText());
                        break;
                    }
                case WebElementType.JQueryCombo:
                    {
                        IEnumerable<object> textObjects = (ReadOnlyCollection<object>)
                            jsExecutor.ExecuteScript("var optionTexts = [];" +
                                                         "var options = arguments[0].options;" +
                                                         "for(var i = 0; i < options.length; i++)" +
                                                         "{" +
                                                         "optionTexts.push(options[i].text);" +
                                                         "}" +
                                                         "return optionTexts;",
                                    webElement);

                        text = textObjects.Select(x => (string)x).ToList();
                        break;
                    }
            }
            return text;
        }
    }

    /// <summary>
    /// Specifies the type of web element to interacted with.
    /// </summary>
    public enum WebElementType
    {
        /// <summary>
        /// Standard HTML {select} element.
        /// </summary>
        HtmlCombo,
        /// <summary>
        /// JQuery combo-box with hidden {select} and a visible associated {input} to interact with.
        /// </summary>
        JQueryCombo,
        /// <summary>
        /// Standard HTML {input type="checkbox"} element.
        /// </summary>
        HtmlCheckbox,
        /// <summary>
        /// JQuery check-box with hidden {input} and a visible associated {label} to interact with.
        /// </summary>
        JQueryCheckbox,
    }
}
