using Ravitej.Automation.Common.Drivers;
using Ravitej.Automation.Common.PageObjects.Session.Navigators;
using Ravitej.Automation.Sample.PageObjects.Pages;
using Ravitej.Automation.Sample.PageObjects.Pages.CreateAccount;

namespace Ravitej.Automation.Sample.PageObjects.Fluent
{
    public class MoonpigSessionNavigatorOnPage : OnPage
    {
        public MoonpigSessionNavigatorOnPage(IDriverSession driverSession)
            : base(driverSession)
        {
        }

        public IMoonpigHome MoonpigHome => ResolvePageObjectAndCheck<IMoonpigHome>();

        public ICreateAcount CreateAccount => ResolvePageObjectAndCheck<ICreateAcount>();

        public ISignIn SignIn => ResolvePageObjectAndCheck<ISignIn>();

        public ITermsAndConditions TermsAndConditions => ResolvePageObjectAndCheck<ITermsAndConditions>();
    }
}