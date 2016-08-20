using Ravitej.Automation.Common.Config;

namespace Ravitej.Automation.Sample.Ui.Tests.Settings
{
    public class MoonpigPersistableSettings : PersistableSettings
    {
        public override void HydrateWithDefaults()
        {
            Username = "someusername";
            Password = "secret!";
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
