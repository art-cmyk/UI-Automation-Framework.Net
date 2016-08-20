using System.Collections.Generic;
using OpenQA.Selenium;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Sample.PageObjects.Fluent;
using Ravitej.Automation.Sample.PageObjects.Pages.Base;

namespace Ravitej.Automation.Sample.PageObjects.Components.Menu.Selenium
{
    public class SubMenu : Interactable, ISubMenu
    {

        #region Fields

        private readonly IMoonpigSession _session;
        private readonly List<ISubMenuItem> _subMenuItems = new List<ISubMenuItem>();
        private readonly IEnumerable<string> _subMenuItemsNames = new List<string>();
        private readonly IWebDriver _driver;
        private readonly IMenuItem _parentMenuItem;

        #endregion

        public SubMenu(IMoonpigSession session, IMenuItem parentMenuItem)
            : base(session)
        {
            _session = session;
            _driver = session.DriverSession.Driver;
            _parentMenuItem = parentMenuItem;
        }

        private ISearchContext Container => GetElementOrThrow(ContainerBy, ContainerDescription);

        private string ContainerDescription => $"Submenu container for {_parentMenuItem.Name} menu item";

        private By ContainerBy
        {
            get
            {
                if (_parentMenuItem.Name.ToLowerInvariant().Equals("cards"))
                {
                    return By.Id("menu-cards");
                } //ideally speak to the devs and fix the need to do this.
                return By.CssSelector($"section.menu-section-{_parentMenuItem.Name}");
            }
        }


        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            return IsElementDisplayed(ContainerBy, ContainerDescription);
        }

        public bool CheckExists(string sSubMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public T Click<T>(string subMenuItemName) where T : IMoonpigBasePage
        {
            throw new System.NotImplementedException();
        }

        public ISubMenuItem Get(string sSubMenuItemName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ISubMenuItem> SubMenuItems { get; private set; }
        public IEnumerable<string> SubMenuItemTexts { get; private set; }
    }
}