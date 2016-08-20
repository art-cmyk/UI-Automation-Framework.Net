namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeSettingItem : FakePersistableSettingsBase
    {
        public string Fred
        {
            get;
            set;
        }

        public override void HydrateWithDefaults()
        {
            base.HydrateWithDefaults();
            Fred = "Name is fred";
        }
    }
}
