using Ravitej.Automation.Common.Config;
using Ravitej.Automation.UI.Tests;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeUiTestBase<TSettings> : UiTestBase<FakeSuiteSettings> where TSettings : IPersistableSettings, new()
    {
        protected FakeUiTestBase()
        {
            TestBaseNamespace = "Ravitej.Automation.Run.UI.Tests";
            TestResultsBaseFolder = "Run";
        }

        protected TSettings TestSettings { get; set; }

        //public void MyTestSettings<T>() where T : IPersistableSettings, new()
        //{
        //    base.GetTestSettingsAndPersistIfDefault<T>();
        //}
    }
}
