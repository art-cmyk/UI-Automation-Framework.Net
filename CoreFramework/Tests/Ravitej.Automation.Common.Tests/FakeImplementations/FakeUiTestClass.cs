using Ravitej.Automation.Common.Config;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeUiTestClass : FakeUiTestBase<FakeSettingItem>
    {
        public void Settings()
        {
            base.GetTestSettingsAndPersistIfDefault<FakeSettingItem>(SettingsType.ProjectBound);
        }
    }
}
