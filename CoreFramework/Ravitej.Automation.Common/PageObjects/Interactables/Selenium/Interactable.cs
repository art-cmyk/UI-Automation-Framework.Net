using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Ravitej.Automation.Common.Dependency;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Common.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.ObjectBuilder2;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Ravitej.Automation.Common.PageObjects.Interactables.Selenium
{
    public abstract partial class Interactable : IInteractable
    {
        /// <summary>
        /// Instance of IWebDriver
        /// </summary>
        protected readonly IWebDriver Driver;

        /// <summary>
        /// Instance of ISession
        /// </summary>
        protected readonly ISession Session;

        public string Name { get; protected set; }

        /// <summary>
        /// The element which contains the main content of the interactable item.
        /// </summary>
        /// <returns>The first matching <see cref="T:OpenQA.Selenium.IWebElement"/> which contains the main content of the page.</returns>
        protected internal virtual IWebElement ContentContainer { get; protected set; }

        protected Interactable(ISession session)
        {
            Session = session;
            Driver = Session.DriverSession.Driver;
        }

        public abstract bool IsDisplayed(bool throwWhenNotDisplayed = false);

        private readonly Func<string, string, string> _associatedElementDesc =
            (elementDesc, associatedElementType) =>
                $"associated {associatedElementType} for {elementDesc}";

        private void _PerformJavaScriptBlur(By @by, string elementDescription)
        {
            var splitBy = @by.ToString().Split(':');
            var elementId = splitBy[1].Trim();
            var isById = splitBy[0].Contains("Id");
            if (isById == false)
            {
                LogX.Warning.Category(GetType().ToString())
                    .Write("Cannot perform blur on {0}({1}) when not using By.Id mechanism.", elementDescription, @by.ToString());
            }
            ((IJavaScriptExecutor)Driver).ExecuteScript($"$('#{elementId}').blur()");
            WriteInfoLogEntry($"Performed JavaScript blur on {elementDescription} textbox with Id: {elementId}");
        }

        private static string GetId(By @by, string elementDescription)
        {
            var splitBy = @by.ToString().Split(':');
            var elementId = splitBy[1].Trim();
            var isById = splitBy[0].Contains("Id");

            if (!isById)
            {
                throw new ArgumentException(
                    $"Cannot perform actions on {elementDescription}({@by.ToString()}) when not using By.Id mechanism.");
            }

            return elementId;
        }

        private void TryClickOrScroll(IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch (InvalidOperationException)
            {
                const string js = "arguments[0].scrollIntoView(true);";
                ((IJavaScriptExecutor)Driver).ExecuteScript(js, element);
                Thread.Sleep(1000); //Don't know why, but need this line for the js to work!
                element.Click();
            }
        }
        /// <summary>
        /// Clicks the element and logs the action. If the element is not in view, it is scrolled into view and then clicked.
        /// </summary>
        /// <param name="element">Element to click on.</param>
        /// <param name="elementDescription">User-friendly description of the element to use in logs and exception messages.</param>
        protected void Click(IWebElement element, string elementDescription)
        {
            TryClickOrScroll(element);
            //SwitchToDefaultContent(); //commenting out to avoid UnhandledAlertException if an alert comes up due to clicking
            WriteInfoLogEntry(ConstructClickedMessage(elementDescription));
        }
        /// <summary>
        /// Gets the ticked status of the given checkbox/radio button element and logs the action.
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected bool IsTicked(IWebElement webElement, string elementDescription)
        {
            var isTicked = webElement.IsTicked();
            WriteInfoLogEntry(ConstructDataFetchedMessage(isTicked.ToString(), elementDescription));
            return isTicked;
        }
        /// <summary>
        /// Gets the inner text of element (other than textox or textrea - returns empty string) without any leading or trailing spaces and logs the action.
        /// </summary>
        /// <param name="element">Element to get text of.</param>
        /// <param name="elementDescription">User-friendly description of the element to use in logs and exception messages.</param>
        /// <returns>Returns a string containing the text of the element.</returns>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        protected string GetText(IWebElement element, string elementDescription)
        {
            var text = element.GetText();
            WriteInfoLogEntry(ConstructDataFetchedMessage(text, elementDescription));
            return text;
        }
        /// <summary>
        /// Hovers (moves mouse pointer) over the given element.
        /// </summary>
        /// <param name="element">Element to hover over</param>
        /// <param name="elementDescription">User-friendly description of the element to use in logs and exception messages.</param>
        protected void HoverOver(IWebElement element, string elementDescription)
        {
            var action = new Actions(Driver);
            action.MoveToElement(element).Build().Perform();
            WriteInfoLogEntry("Hovered over {0}", elementDescription);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructNotFoundErrorMessage(string elementDescription)
        {
            return $"Could not find {elementDescription} on {Name} page";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enteredValue"></param>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructDataEnteredMessage(string enteredValue, string elementDescription)
        {
            return $"Entered '{enteredValue}' into {elementDescription} on {Name} page";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fetchedValue"></param>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructDataFetchedMessage(string fetchedValue, string elementDescription)
        {
            return $"Fetched value '{fetchedValue}' from {elementDescription} on {Name} page";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearedValue"></param>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructDataClearedMessage(string clearedValue, string elementDescription)
        {
            return $"Cleared text '{clearedValue}' from {elementDescription} on {Name} page";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fetchedValue"></param>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructDataFetchedMessage(IEnumerable<string> fetchedValue, string elementDescription)
        {
            return $"Fetched values '{fetchedValue.JoinStrings(", ")}' from {elementDescription} on {Name} page";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructClickedMessage(string elementDescription)
        {
            return $"Clicked {elementDescription} on {Name} page";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementDescription"></param>
        /// <param name="ticked"></param>
        /// <returns></returns>
        protected string ConstructCheckboxClickedMessage(string elementDescription, bool ticked)
        {
            return string.Format(ticked ? "Ticked {0} on {1} page" : "Unticked {0} on {1} page", elementDescription, Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enteredValue"></param>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructDataSelectedMessage(string enteredValue, string elementDescription)
        {
            return $"Selected '{enteredValue}' from {elementDescription} on {Name} page";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enteredValue"></param>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructDataValueSelectedMessage(string enteredValue, string elementDescription)
        {
            return $"Selected option with value '{enteredValue}' from {elementDescription} on {Name} page";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pastedText"></param>
        /// <param name="elementDescription"></param>
        /// <returns></returns>
        protected string ConstructDataPastedMessage(string pastedText, string elementDescription)
        {
            return $"Pasted '{pastedText}' from system clipboard into {elementDescription} on {Name} page";
        }
        /// <summary>
        /// Gets an IWebElement from the iPDetail frame using its id.
        /// This method is virtual as it is necessary in some cases to change the way that an element is accessed, such as when in a different frameset than usual.
        /// </summary>
        /// <param name="elementId">The ID of the element to get</param>
        /// <param name="elementDescription">User-friendly description of the element to use in logs and exception messages.</param>
        /// <param name="overrideTargetFrameName"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">If no element matches the criteria.</exception>
        /// <exception cref="NoSuchFrameException">If the frame with Id or name specified in framecannot be found.</exception>
        protected virtual IWebElement GetElementByIdOrThrow(string elementId, string elementDescription,
            string overrideTargetFrameName = "iPDetail")
        {
            return GetElementOrThrow(By.Id(elementId), elementDescription, overrideTargetFrameName);
        }

        /// <summary>
        /// Write an Information type log entry
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected void WriteInfoLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteInfoLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Write a Verbose type log entry
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected void WriteVerboseLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteVerboseLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Write a Warning type log entry.
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected void WriteWarningLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteWarningLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Write an Error type log entry.
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected void WriteErrorLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteErrorLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Write a Critical type log entry.
        /// </summary>
        /// <param name="message">Message to write to the log.</param>
        /// <param name="args">Optional parameters to substitute in the <paramref name="message"/>.</param>
        protected void WriteCriticalLogEntry(string message, params object[] args)
        {
            Logging.Logging.WriteCriticalLogEntry(GetType().ToString(), message, args);
        }

        /// <summary>
        /// Selects either the first frame on the page or the main document when a page contains iFrames.
        /// </summary>
        protected void SwitchToDefaultContent()
        {
            Driver.SwitchTo().DefaultContent();
        }


        /// <summary>
        /// Gets the element value using its Id.  If no element can be found an exception is thrown.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="elementDescription">User-friendly description of the element to use in logs and exception messages.</param>
        /// <param name="overrideTargetFrameName"></param>
        /// <returns></returns>
        protected string GetTextById(string elementId, string elementDescription,
            string overrideTargetFrameName = "iPDetail")
        {
            return GetText(By.Id(elementId), elementDescription, overrideTargetFrameName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="elementDescription">User-friendly description of the element to use in logs and exception messages.</param>
        /// <param name="newValue"></param>
        /// <param name="blurFieldPostUpdate"></param>
        /// <param name="overrideTargetFrameName"></param>
        protected void EnterTextById(string elementId, string elementDescription, string newValue,
            bool blurFieldPostUpdate = false, string overrideTargetFrameName = "iPDetail")
        {
            EnterText(By.Id(elementId), elementDescription, newValue, blurFieldPostUpdate, overrideTargetFrameName);
        }
        /// <summary>
        /// Pauses the current script execution
        /// </summary>
        /// <param name="duration">How long to wait</param>
        protected void PauseExecution(Pause.ScriptWaitDuration duration)
        {
            Pause.PauseExecution(duration);
        }
        /// <summary>
        /// Pauses the current script execution and returns an instance of the specified type so as to be fluent
        /// </summary>
        /// <typeparam name="TPageType">The type of IRunBasePage to return</typeparam>
        /// <param name="duration">How long to wait for</param>
        /// <returns>An instance of a type to work with</returns>
        protected TPageType PauseExecution<TPageType>(Pause.ScriptWaitDuration duration) where TPageType : IInteractable
        {
            Pause.PauseExecution(duration);
            return AppWide.Instance.Resolve<TPageType>();
        }

        /// <summary>
        /// Private method to check that the page is displayed. Always ensures 
        /// that the browser is set to the outer most frame prior to making 
        /// the page check, so as to be in a consistent state.
        /// </summary>
        /// <returns></returns>
        private bool IsPageDisplayed()
        {
            SwitchToDefaultContent();
            var result = IsDisplayed();
            return result;
        }

        /// <summary>
        /// Ask the browser to perform any appropriate checks to determine if it is ready
        /// </summary>
        /// <param name="timetaken"></param>
        /// <param name="timeout">What is the timeout for the waiting to use - default is 10 seconds</param>
        /// <param name="pollingInterval">How frequently should the method check with the browser - default is 250 miliseconds</param>
        /// <param name="innerPageContainerId">Id of the frame that contains the inner page</param>
        /// <returns></returns>
        protected virtual bool WaitForBrowserToReportReady(out long timetaken, int timeout = 10000, int pollingInterval = 250, string innerPageContainerId = null)
        {
            bool pageFullyLoaded = Driver.WaitForPageFullyLoaded(timeout, out timetaken, pollingInterval, innerPageContainerId);

            WriteVerboseLogEntry(pageFullyLoaded ? "According to the browser, '{0}' page was fully loaded in {1} milliseconds." : "According to the browser, '{0}' page was NOT fully loaded {1} milliseconds.", Name, timetaken);

            return pageFullyLoaded;
        }

        /// <summary>
        /// Using both the browser and, any explicit on-page checks, wait for the page and return a value indicating whether the page is displayed and fully loaded
        /// </summary>
        /// <param name="performOnPageCheck">Should the framework use the PageObject's 'IsDisplayed' method to assert whether it is on the page or not, in addition to the javascript checks</param>
        /// <param name="timeout">What is the timeout for the waiting to use - default is 10 seconds</param>
        /// <param name="pollingInterval">How frequently should the method check with the browser - default is 250 miliseconds</param>
        /// <param name="innerPageContainerId">Id of the frame that contains the inner page</param>
        /// <returns></returns>
        protected virtual bool WaitForPageToBeFullyLoaded(bool performOnPageCheck = false, int timeout = 10000, int pollingInterval = 250, string innerPageContainerId = null)
        {
            long timetaken;

            var pageFullyLoaded = WaitForBrowserToReportReady(out timetaken, timeout, pollingInterval, innerPageContainerId);

            // if the client code reports that the page was fully loaded and, the entry code asks for a deeper check
            if (pageFullyLoaded && performOnPageCheck)
            {
                WriteVerboseLogEntry("Performing a deeper fully loaded check for '{0}' page.", Name);

                // set the flag to the value of the IsDisplayed method - it may already be there at this time
                pageFullyLoaded = IsPageDisplayed();

                if (!pageFullyLoaded)
                {
                    // log the fact that the page was not fully loaded at this time
                    WriteVerboseLogEntry("Deeper check for '{0}' page reported that it was not fully loaded yet.", Name);

                    // determine how much time is left, taking into account the elapsed time and the allowed timeout value
                    long remainingTime = timeout - timetaken;

                    // start a new stopwatch instance and 
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    // while the page is not loaded and, there's still some time left
                    while (pageFullyLoaded == false && stopwatch.Elapsed.TotalMilliseconds <= remainingTime)
                    {
                        // sleep for the polling interval value
                        Thread.Sleep(pollingInterval);

                        // check the page again
                        pageFullyLoaded = IsPageDisplayed();
                    }

                    // stop the clock
                    stopwatch.Stop();

                    // calculate the total time taken
                    timetaken += stopwatch.ElapsedMilliseconds;

                    // log the finding
                    WriteVerboseLogEntry(pageFullyLoaded ? "Deeper check for '{0}' page reported a successful check in {1} ms" : "Deeper check for '{0}' page reported an incomplete check in {1} ms", Name, timetaken);
                }
                else
                {
                    WriteVerboseLogEntry("The browser reported that '{0}' page was fully loaded and the IsDisplayed method was validated successfully.", Name);
                }
            }

            return pageFullyLoaded;
        }

        private static void _SetClipboardText(string textToPaste)
        {
            var thread = new Thread(() => Clipboard.SetText(textToPaste));
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
        }
    }
}