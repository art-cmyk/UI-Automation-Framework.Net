namespace Ravitej.Automation.Common.PageObjects.SelfValidation
{
    /// <summary>
    /// Indicates that this class and all decendants should be explicitly ignored when a parent element is self-validated
    /// This allows for an object to be defined but not form a part of a parent validation.
    /// 
    /// For example:
    ///     ClassA contains its own elements but also contains a number of 'widgets'.  These widgets may or may not be visible on the screen.
    ///     Decorating ChildClass as IExcludeElementsWhenAChildObject would mean that any elements defined within itself are explicitly ignored and
    ///     should be validated itself, rather than with the parent
    /// </summary>
    public interface IExcludeElementsWhenAChildObject
    {
    }
}
