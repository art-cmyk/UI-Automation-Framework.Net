using Ravitej.Automation.Common.Config;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakePersistableSettingsBase : PersistableSettings
    {
        public override void HydrateWithDefaults()
        {
            UserName = "admin";
            Password = "test1234";
            InstallationId = "20022121";
        }

        public string InstallationId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
