using Ravitej.Automation.Common.PageObjects.Session;

namespace Ravitej.Automation.Sample.PageObjects.Fluent
{
    public interface IMoonpigSession : ISession
    {
        new MoonpigSessionNavigatorGoTo GoTo { get; }
        new MoonpigSessionNavigatorOnPage OnPage { get; }
    }
}