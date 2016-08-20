using System;
using System.Collections.Generic;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Common.PageObjects.Services;
using Ravitej.Automation.Common.PageObjects.Session;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Ravitej.Automation.Common.PageObjects.Components.ModalDialog.Selenium
{
    /// <summary>
    /// Provides the <b>abstract</b> base class for all modal dialogs in the application under test (AUT).
    /// </summary>
    public abstract class ModalDialog : Interactable, IModalDialog
    {
        /// <summary>
        /// Initialises a new instance of <see cref="T:ModalDialog"/>
        /// </summary>
        /// <param name="session">Instance of <see cref="ISession"/> representing current session.</param>
        protected ModalDialog(ISession session)
            : base(session)
        {
        }

        /// <summary>
        /// Gets the locating mechanism for <see cref="Interactable.ContentContainer"/>, element containing this modal dialog.
        /// </summary>
        protected abstract By ContainerSelector { get; }

        /// <summary>
        /// Gets the locating mechanism for the overlay that hides the background page when modal dialog is displayed
        /// </summary>
        protected abstract By OverlaySelector { get; }

        public abstract string GetTitle();

        public abstract IEnumerable<string> GetButtons();

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            using (var checker = new OnPageChecker(Driver, Name))
            {
                return checker.ElementExistsOnPage(ContainerSelector)
                    .CustomCheckOnPage(IsElementDisplayed(ContainerSelector, $"{Name} modal dialog"))
                    .Confirm(throwWhenNotDisplayed);
            }
        }

        public T HitEscapeKey<T>() where T : IInteractable
        {
            var action = new Actions(Driver);
            action.SendKeys(Keys.Escape).Build().Perform();
            WriteInfoLogEntry("Sent 'Esc' key to the browser on {0} modal dialog.", Name);
            return Session.OnPage.ResolvePageObjectAndCheck<T>();
        }

        /// <exception cref="InvalidOperationException">When <see cref="OverlaySelector"/> is null.</exception>
        public void WaitForOverlayGone(int timeoutSeconds)
        {
            if (OverlaySelector != null)
            {
                try
                {
                    Driver.WaitFor(d => d.FindElementSafe(OverlaySelector) == null, timeoutSeconds);
                }
                catch (WebDriverTimeoutException)
                {
                    //Do nothing. Let the execution continue.
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot perform wait operation when 'OverlaySelector' is null.");
            }
        }
    }
}