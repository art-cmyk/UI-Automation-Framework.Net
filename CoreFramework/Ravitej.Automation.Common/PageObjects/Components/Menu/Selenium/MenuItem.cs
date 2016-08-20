using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Common.PageObjects.Session;

namespace Ravitej.Automation.Common.PageObjects.Components.Menu.Selenium
{
    public class MenuItem : Interactable, IMenuItem
    {
        private readonly IWebDriver _driver;
        private readonly ISession _session;

        public MenuItem(ISession session, string name)
            : base(session)
        {
            _session = session;
            _driver = session.DriverSession.Driver;
            Name = name;
        }

        private IWebElement MenuItemElement => GetElementOrThrow(MenuItemBy, MenuItemDescription);
        private string MenuItemDescription => $"{Name} menu item";

        private By MenuItemBy => new ByChained(By.CssSelector("nav.navbar"), By.LinkText(Name));

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            return IsElementDisplayed(MenuItemBy, MenuItemDescription);
        }

        public void Click()
        {
            Click(MenuItemBy, MenuItemDescription);
        }

        public void HoverOver()
        {
            HoverOver(MenuItemElement, MenuItemDescription);
        }

        public void MoveAway()
        {
            throw new System.NotImplementedException();
        }
    }
}