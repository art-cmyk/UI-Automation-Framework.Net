namespace Ravitej.Automation.Common.Config.SettingsUpdater
{
    /// <summary>
    /// Interface that exposes actions that can be performed on the settings file
    /// </summary>
    public interface ISettingsUpdater
    {
        /// <summary>
        /// Update any settings specific to the test suite with the values in the app.config.
        /// 
        /// Implementations can also perform any suite specific settings in here over and above the base methods.
        /// </summary>
        void UpdateTestSuiteConfigFile();

        /// <summary>
        /// Update the app.config  with the values in the app.config.
        /// </summary>
        void UpdateAppConfigFile();
    }
}