using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace OpenQA.Selenium
{
    /// <summary>
    /// Extension methods for the web driver
    /// </summary>
    public static class WebDriverExtensions
    {
        /// <summary>
        /// Wait until the element is visible
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutSeconds"></param>
        public static void WaitForElementIsVisible(this IWebDriver driver, By by, int timeoutSeconds)
        {
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            new WebDriverWait(driver, timeout).Until(ExpectedConditions.ElementIsVisible(by));
        }

        /// <summary>
        /// Wait for the element to not be visible
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutSeconds"></param>
        public static void WaitForElementNotVisible(this IWebDriver driver, By by, int timeoutSeconds)
        {
            WaitFor(driver,
                delegate(IWebDriver d)
                {
                    var element = d.FindElementSafe(by);
					bool result;
                    try
                    {
                        result = element == null || element.Displayed == false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        result = false;
                    }
                    return result;
                }, timeoutSeconds);
        }

        /// <summary>
        /// Wait until the element exists
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutSeconds"></param>
        public static void WaitForElementExists(this IWebDriver driver, By by, int timeoutSeconds)
        {
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            new WebDriverWait(driver, timeout).Until(ExpectedConditions.ElementExists(by));
        }

        /// <summary>
        /// Wait for a period of time
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds"></param>
        public static void WaitFor(this IWebDriver driver, Func<IWebDriver, bool> condition, int timeoutSeconds)
        {
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var waitFor = new WebDriverWait(driver, timeout);
            waitFor.IgnoreExceptionTypes(typeof(ElementNotVisibleException),
                                      typeof(NoSuchElementException));
            waitFor.Until(condition);
        }

        /// <summary>
        /// Wait for a period of time
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="condition"></param>
        /// <param name="timeoutSeconds"></param>
        public static void WaitFor(this IWebDriver driver, Func<IWebDriver, IWebElement> condition, int timeoutSeconds)
        {
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var waitFor = new WebDriverWait(driver, timeout);
            waitFor.IgnoreExceptionTypes(typeof(ElementNotVisibleException),
                                      typeof(NoSuchElementException));
            waitFor.Until(condition);
        }

        /// <summary>
        /// Move the mouse to the centre of the page
        /// </summary>
        /// <param name="oDriver"></param>
        public static void MoveMouseToCentreOfPage(this IWebDriver oDriver)
        {
            var oBodyElement = oDriver.FindElement(By.TagName("body"));
            ((RemoteWebDriver)oDriver).Mouse.MouseMove(((RemoteWebElement)oBodyElement).Coordinates,
                oBodyElement.Size.Width / 2, oBodyElement.Size.Height / 2);
        }

        /// <summary>
        /// Click on the centre of the page
        /// </summary>
        /// <param name="oDriver"></param>
        public static void ClickCentreOfPage(this IWebDriver oDriver)
        {
            var oBodyElement = oDriver.FindElement(By.TagName("body"));
            ((RemoteWebDriver)oDriver).Mouse.MouseMove(((RemoteWebElement)oBodyElement).Coordinates,
                oBodyElement.Size.Width / 2, oBodyElement.Size.Height / 2);
            ((RemoteWebDriver)oDriver).Mouse.MouseDown(((RemoteWebElement)oBodyElement).Coordinates);
            ((RemoteWebDriver)oDriver).Mouse.MouseUp(((RemoteWebElement)oBodyElement).Coordinates);
        }

        /// <summary>
        /// Get a checkbox
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public static IWebElement GetCheckbox(this IWebDriver driver, string id, string description, string pageName)
        {
            return driver.FindElementOrThrow(By.Id(id),
                $"Could not find {description} checkbox on {pageName} page.");
        }

        /// <summary>
        /// Wait for the page to be fully loaded
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeout"></param>
        /// <param name="timeTaken"></param>
        /// <param name="pollingInterval"></param>
        /// <returns></returns>
        public static bool WaitForPageFullyLoaded(this IWebDriver driver, int timeout, out long timeTaken, int pollingInterval = 1000, string innerPageElement = null)
        {
            var javascript = innerPageElement == null ? _GetWaitForPageJavascript("true") : _GetWaitForInnerPageJavascript(innerPageElement, "true");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var pageLoaded = (bool)((IJavaScriptExecutor)driver).ExecuteScript(javascript);
            while (pageLoaded == false && stopwatch.Elapsed.TotalMilliseconds <= timeout)
            {
                Thread.Sleep(pollingInterval);
                pageLoaded = (bool)((IJavaScriptExecutor)driver).ExecuteScript(javascript);
            }
            stopwatch.Stop();
            LogX.Info.Category("WebDriverExtensions").Write("Waited for {0} milliseconds for page to finish loading", stopwatch.ElapsedMilliseconds);
            timeTaken = stopwatch.ElapsedMilliseconds;
            return pageLoaded;
        }

        /// <summary>
        /// Wait for the next page to start loading
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="timeout"></param>
        /// <param name="timeTaken"></param>
        /// <param name="pollingInterval"></param>
        /// <returns></returns>
        public static bool WaitForNextPageStartLoading(this IWebDriver driver, int timeout, out long timeTaken, int pollingInterval = 1000, string innerPageElement = null)
        {
            var javascript = innerPageElement == null ? _GetWaitForPageJavascript("false") : _GetWaitForInnerPageJavascript(innerPageElement, "false");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var pageLoaded = (bool)((IJavaScriptExecutor)driver).ExecuteScript(javascript);
            while (pageLoaded && stopwatch.Elapsed.TotalMilliseconds <= timeout)
            {
                Thread.Sleep(pollingInterval);
                pageLoaded = (bool)((IJavaScriptExecutor)driver).ExecuteScript(javascript);
            }
            stopwatch.Stop();
            LogX.Info.Category("WebDriverExtensions").Write("Waited for {0} milliseconds for next page to start loading", stopwatch.ElapsedMilliseconds);
            timeTaken = stopwatch.ElapsedMilliseconds;
            return pageLoaded;
        }

        private static string _GetWaitForPageJavascript(string pageloadComplete) //bool true send True to the server and JS is case sensitive. Hence, using string and passing "true" and "false"
        {
            string javascript = "var test = function()" +
                                      "{" +
                                      "var result = false;" +
                                      "try" +
                                      "{" +
                                $"result = (top.document.getElementsByTagName('body')[0].getAttribute(\"data-automation-pageload-complete\") == \"{pageloadComplete}\")" +
                                      "}" +
                                      "catch(ex)" +
                                      "{" +
                                      "}" +
                                      "return result;" +
                                      "}; return test();";
            return javascript;
        }

        private static string _GetWaitForInnerPageJavascript(string inner, string loadComplete) //loadComplete value should either be "true" or "false"
        {
            string javascript = "var test = function()" +
                "{" +
                    "var result = false;" +
                    "try" +
                    "{" +
                                $"result = (top.frames[\"{inner}\"].contentDocument.getElementsByTagName('body')[0].getAttribute(\"data-automation-pageload-complete\") == \"{loadComplete}\")" +
                    "}" +
                    "catch(ex)" +
                    "{" +
                    "}" +
                    "return result;" +
                "}; return test();";
            return javascript;
            }

        //public static bool WaitForCwcPageFullyLoaded(this IWebDriver driver, int timeout, out long timeTaken, int pollingInterval = 250)
        //{
        //    const string javascript = "var test = function()" +
        //        "{" +
        //            "var result = false;" +
        //            "try" +
        //            "{" +
        //                "result = (top.frames[\"iPDetail\"].contentDocument.getElementsByTagName('BODY')[0].getAttribute(\"data-automation-pageload-complete\") == \"true\")" +
        //            "}" +
        //            "catch(ex)" +
        //            "{" +
        //            "}" +
        //            "return result;" +
        //        "}; return test();";
        //    var pageLoaded = (bool)((IJavaScriptExecutor)driver).ExecuteScript(javascript);
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    while (pageLoaded == false && stopwatch.Elapsed.TotalMilliseconds <= timeout)
        //    {
        //        Thread.Sleep(pollingInterval);
        //        pageLoaded = (bool)((IJavaScriptExecutor)driver).ExecuteScript(javascript);
        //    }
        //    stopwatch.Stop();
        //    LogX.Info.Category("WebDriverExtensions").Write("Waited for {0} milliseconds", stopwatch.ElapsedMilliseconds);
        //    timeTaken = stopwatch.ElapsedMilliseconds;
        //    return pageLoaded;
        //}

        /// <summary>
        /// Take a screenshot
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static Screenshot TakeScreenshot(this IWebDriver driver)
        {
            var screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver != null)
            {
                Screenshot screenshot;

                try
                {
                    screenshot = screenshotDriver.GetScreenshot();
                }
                catch (InvalidOperationException)
                {
                    Thread.Sleep(500);
                    screenshot = screenshotDriver.GetScreenshot();
                }

                return screenshot;
            }

            return null;
        }

        /// <summary>
        /// Check to see if an alert box is displayed
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="dialogMessage"></param>
        /// <returns></returns>
        public static bool IsAlertBoxDisplayed(this IWebDriver driver, out string dialogMessage)
        {
            dialogMessage = string.Empty;
            try
            {
                string alertText = driver.SwitchTo().Alert().Text;
                dialogMessage = alertText;
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
    }
}
