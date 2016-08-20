using Ravitej.Automation.Common.PageObjects.Interactables;

namespace Ravitej.Automation.Sample.PageObjects.Components.Menu
{
    public interface IMenuItem : IInteractable
    {
        void Click();
        ISubMenu HoverOver();
        void MoveAway();
    }
}