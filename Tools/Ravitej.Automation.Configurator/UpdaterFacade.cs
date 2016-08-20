using Ravitej.Automation.Configure.FileUpdaters;

namespace Ravitej.Automation.Configure
{
    internal static class UpdaterFacade
    {
        public static void UpdateSettingsConfig(string targetAssembly)
        {
            var updater = UpdaterFactory.GetUpdater(targetAssembly);
            updater.UpdateAppConfigFile();
            updater.UpdateTestSuiteConfigFile();
        }
    }
}
