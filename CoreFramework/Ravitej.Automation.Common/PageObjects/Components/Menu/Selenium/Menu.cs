using System.Collections.Generic;
using OpenQA.Selenium;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Common.PageObjects.Session;

namespace Ravitej.Automation.Common.PageObjects.Components.Menu.Selenium
{
    public class Menu : IMenu
    {
        #region Fields

        private readonly IWebDriver _driver;
        private readonly ISession _session;

        #endregion

        #region Constructors/Finalisers

        public Menu(ISession session)
        {
            _session = session;
            _driver = session.DriverSession.Driver;
        }

        #endregion

        public bool CheckExists(string sMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public void HoverOver(string sMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public void MoveAway(string sMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public T Click<T>(string menuItemName) where T : IInteractable
        {
            new MenuItem(_session, menuItemName).Click();
            return _session.OnPage.ResolvePageObjectAndCheck<T>();
        }

        public IMenuItem Get(string sMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public IMenuItem SelectedItem { get; private set; }
        public IEnumerable<IMenuItem> MenuItems { get; private set; }
    }
}