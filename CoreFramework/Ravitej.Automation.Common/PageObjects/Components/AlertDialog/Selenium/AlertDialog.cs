using System;
using System.Collections.Generic;
using Ravitej.Automation.Common.Dependency;
using Ravitej.Automation.Common.PageObjects.Components.BusyIndicator;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Common.PageObjects.Session;
using Ravitej.Automation.Common.Utilities;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.PageObjects.Components.AlertDialog.Selenium
{
    /// <summary>
    /// Implements <see cref="IAlertDialog"/> which provides interface through which the user can manipulate JavaScript alerts.
    /// </summary>
    public class AlertDialog : IAlertDialog
    {
        private readonly ISession _session;
        private readonly IWebDriver _driver;
        public string Name { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="T:AlertDialog"/>.
        /// </summary>
        public AlertDialog()
        {
            Name = "Alert dialog";
            _session = AppWide.Instance.Resolve<ISession>();
            _driver = _session.DriverSession.Driver;
        }

        public void AcceptAlert()
        {
            _ActOnAlert(true);
        }

        public T AcceptAlert<T>() where T : IInteractable
        {
            AcceptAlert();
            return AppWide.Instance.Resolve<T>();
        }

        public void DismissAlert()
        {
            _ActOnAlert(false);
        }

        public T DismissAlert<T>() where T : IInteractable
        {
            DismissAlert();
            return AppWide.Instance.Resolve<T>();
        }

        public string GetAlertText()
        {
            return IsDisplayed() ? _SwitchToAlert().Text.Trim() : null;
        }


        /// <summary>
        /// Gets a value indicating whether alert is displayed or not.
        /// </summary>
        /// <param name="throwWhenNotDisplayed">Indicate whether <see cref="NoAlertPresentException"/> should be thrown if alert is not displayed.</param>
        /// <returns>True if alert is displayed. False if alert is not displayed and <paramref name="throwWhenNotDisplayed"/> is set to false.</returns>
        /// <exception cref="NoAlertPresentException">Thrown when an alert is not found and <paramref name="throwWhenNotDisplayed"/> is true.</exception>
        public bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            Pause.PauseExecution(Pause.ScriptWaitDuration.Long);
            try
            {
                _SwitchToAlert();
                Logging.Logging.WriteVerboseLogEntry(GetType().ToString(), "Alert is displayed.");
                return true;
            }
            catch (NoAlertPresentException)
            {
                Logging.Logging.WriteVerboseLogEntry(GetType().ToString(), "Alert is not displayed.");
                if (throwWhenNotDisplayed)
                {
                    throw;
                }
                return false;
            }
        }

        private void LaunchWaitSpinner()
        {
            var alertParams = new Dictionary<string, object> { { "session", _session } };
            AppWide.Instance.Resolve<IProcessingSpinner>(alertParams).Wait(TimeSpan.FromSeconds(15));
        }

        private IAlert _SwitchToAlert()
        {
            return _driver.SwitchTo().Alert();
        }

        private void _ActOnAlert(bool accept)
        {
            var alertText = GetAlertText();
            if (string.IsNullOrEmpty(alertText))
            {
                Pause.PauseExecution(Pause.ScriptWaitDuration.Long);
                alertText = GetAlertText();
            }
            if (accept)
            {
                _SwitchToAlert().Accept();
            }
            else
            {
                _SwitchToAlert().Dismiss();
            }
            _driver.SwitchTo().DefaultContent();

            LaunchWaitSpinner();
            Logging.Logging.WriteInfoLogEntry(GetType().ToString(), "{0} the alert with message: {1}", accept ? "Accepted" : "Dismissed", alertText);
        }
    }
}
