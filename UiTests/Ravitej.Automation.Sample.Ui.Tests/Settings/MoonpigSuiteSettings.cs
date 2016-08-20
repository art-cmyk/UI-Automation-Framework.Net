using System;
using Ravitej.Automation.Common.Config.SuiteSettings;

namespace Ravitej.Automation.Sample.Ui.Tests.Settings
{
    public class MoonpigSuiteSettings : SuiteSettings
    {
        public string HomePageUrl
        {
            get;
            set;
        }

        public string FlowersPageUrl
        {
            get;
            set;
        }

        public override string GetLaunchPage(int targetPage)
        {
            var targetPageAsEnum = ((LaunchPage)targetPage);

            switch (targetPageAsEnum)
            {
                case LaunchPage.HomePage:
                    {
                        return HomePageUrl;
                    }
                case LaunchPage.FlowersPage:
                    {
                        return FlowersPageUrl;
                    }
                default:
                    {
                        throw new ArgumentException("Unknown Target Page", nameof(targetPage));
                    }
            }
        }

        public override string GetLaunchPage(int targetPage, ILaunchPageHandler launchPageHandler)
        {
            throw new NotImplementedException();
        }

        public override void HydrateWithDefaults()
        {
            base.HydrateWithDefaults();

            HomePageUrl = "https://www.moonpig.com/uk/";
            FlowersPageUrl = "https://www.moonpig.com/uk/Gift/Flowers/";
        }
    }
}
