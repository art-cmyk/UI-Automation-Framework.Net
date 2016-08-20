using System;
using Ravitej.Automation.Common.Config.AppUnderTest;
using Ravitej.Automation.Common.Config.DriverSession;
using Ravitej.Automation.Common.Config.SuiteSettings;

namespace Ravitej.Automation.Common.Tests.FakeImplementations
{
    public class FakeSuiteSettings : ISuiteSettings
    {
        public string SettingId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DriverSettings WebDriverSettings
        {
            get
            {
                return new DriverSettings().SelfHydrate();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public AutSettings ApplicationUnderTestSettings 
        {
            get
            {
                return new AutSettings().SelfHydrate();
            }
            set
            {
                throw new NotImplementedException();
            } 
        }

        public void AssignSettingId(string settingId)
        {
            throw new NotImplementedException();
        }

        public string GetLaunchPage(int targetPage)
        {
            throw new NotImplementedException();
        }

        public string GetLaunchPage(int targetPage, ILaunchPageHandler launchPageHandler)
        {
            throw new NotImplementedException();
        }

        public void HydrateWithDefaults()
        {
            throw new NotImplementedException();
        }
    }
}
