using System.Collections.Generic;
using OpenQA.Selenium;
using Ravitej.Automation.Common.PageObjects.Interactables;
using Ravitej.Automation.Sample.PageObjects.Fluent;

namespace Ravitej.Automation.Sample.PageObjects.Components.Menu.Selenium
{
    public class Menu : IMenu
    {
        #region Fields

        //private readonly IEnumerable<string> _menuItemNames;
        private readonly IWebDriver _driver;
        private readonly IMoonpigSession _moonpigSession;

        #endregion

        #region Constructors/Finalisers

        public Menu(IMoonpigSession moonpigSession)
        {
            _moonpigSession = moonpigSession;
            _driver = moonpigSession.DriverSession.Driver;
            //_menuItemNames = _GetMenuItemNames();
        }

        #endregion

        public bool CheckExists(string sMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public ISubMenu HoverOver(string sMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public void MoveAway(string sMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public T Click<T>(string menuItemName) where T : IInteractable
        {
            new MenuItem(_moonpigSession, menuItemName).Click();
            return _moonpigSession.OnPage.ResolvePageObjectAndCheck<T>();
        }

        public IMenuItem Get(string sMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public IMenuItem SelectedItem { get; private set; }
        public IEnumerable<IMenuItem> MenuItems { get; private set; }
    }
}