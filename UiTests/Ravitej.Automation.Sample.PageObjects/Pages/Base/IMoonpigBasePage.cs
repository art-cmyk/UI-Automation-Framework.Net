using Ravitej.Automation.Common.PageObjects.Components.BasePage;
using Ravitej.Automation.Sample.PageObjects.Components.Menu;
using Ravitej.Automation.Sample.PageObjects.Components.Search;
using Ravitej.Automation.Sample.PageObjects.Components.TopBar;

namespace Ravitej.Automation.Sample.PageObjects.Pages.Base
{

    public interface IMoonpigBasePage : IBasePage
    {
        IMenu Menu { get; }
        ISearch Search { get; }
        ITopBar TopBar { get; }
        //footer etc.
    }
}