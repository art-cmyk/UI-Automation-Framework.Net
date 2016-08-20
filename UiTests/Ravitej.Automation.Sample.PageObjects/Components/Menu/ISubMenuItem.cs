namespace Ravitej.Automation.Sample.PageObjects.Components.Menu
{
    public interface ISubMenuItem
    {
        string LinkText { get; }
        string LinkUrl { get; }
        string Name { get; }
        void Click();
    }
}