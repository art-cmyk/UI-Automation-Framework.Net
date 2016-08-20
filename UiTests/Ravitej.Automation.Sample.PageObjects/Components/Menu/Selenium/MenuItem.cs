using OpenQA.Selenium;
using Ravitej.Automation.Common.PageObjects.Interactables.Selenium;
using Ravitej.Automation.Sample.PageObjects.Fluent;

namespace Ravitej.Automation.Sample.PageObjects.Components.Menu.Selenium
{
    public class MenuItem : Interactable, IMenuItem
    {
        private readonly IWebDriver _driver;
        private readonly IMoonpigSession _moonpigSession;

        public MenuItem(IMoonpigSession moonpigSession, string name)
            : base(moonpigSession)
        {
            _moonpigSession = moonpigSession;
            _driver = moonpigSession.DriverSession.Driver;
            Name = name;
        }

        private IWebElement MenuItemElement => GetElementOrThrow(MenuItemBy, MenuItemDescription);

        private string MenuItemDescription => $"{Name} menu item";

        private By MenuItemBy => By.CssSelector($"#navigation a#menu-item-{Name.ToLowerInvariant()}");

        public override bool IsDisplayed(bool throwWhenNotDisplayed = false)
        {
            return IsElementDisplayed(MenuItemBy, MenuItemDescription);
        }

        public void Click()
        {
            Click(MenuItemBy, MenuItemDescription);
        }

        public ISubMenu HoverOver()
        {
            HoverOver(MenuItemElement, MenuItemDescription);
            return new SubMenu(_moonpigSession, this);
        }

        public void MoveAway()
        {
            throw new System.NotImplementedException();
        }
    }
}