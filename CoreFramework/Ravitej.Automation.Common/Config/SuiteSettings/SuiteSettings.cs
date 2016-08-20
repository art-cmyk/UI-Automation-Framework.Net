using System;
using Ravitej.Automation.Common.Config.AppUnderTest;
using Ravitej.Automation.Common.Config.DriverSession;

namespace Ravitej.Automation.Common.Config.SuiteSettings
{
    /// <summary>
    /// Provides the <b>abstract</b> base class for common settings applicable to a suite of tests.
    /// </summary>
    [Serializable]
    public abstract class SuiteSettings : PersistableSettings, ISuiteSettings
    {
        public abstract string GetLaunchPage(int targetPage);

        public abstract string GetLaunchPage(int targetPage, ILaunchPageHandler launchPageHandler);

        public override void HydrateWithDefaults()
        {
            WebDriverSettings = new DriverSettings().SelfHydrate();
            ApplicationUnderTestSettings = new AutSettings().SelfHydrate();
        }

        public DriverSettings WebDriverSettings { get; set; }
        public AutSettings ApplicationUnderTestSettings { get; set; }
    }
}
