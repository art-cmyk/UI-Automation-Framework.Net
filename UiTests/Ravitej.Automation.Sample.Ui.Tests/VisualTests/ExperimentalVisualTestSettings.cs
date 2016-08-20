using Ravitej.Automation.Sample.Ui.Tests.Settings;

namespace Adp.Automation.Run.UI.Tests.Visual
{
    public class ExperimentalVisualTestSettings : MoonpigPersistableSettings
    {
        public override void HydrateWithDefaults()
        {
            base.HydrateWithDefaults();

            //Username = "adpadmin";
            //Password = "test1234";
            Heights = new[] { 1080, 900, 768, 1024, 600 };
            Widths = new[] { 1920, 1440, 1366, 1280, 800 };
        }

        public int[] Heights { get; set; }

        public int[] Widths { get; set; }
    }
}
