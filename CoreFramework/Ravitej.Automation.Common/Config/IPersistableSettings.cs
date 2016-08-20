namespace Ravitej.Automation.Common.Config
{
    /// <summary>
    /// Represents a settings object that can be persisted.
    /// </summary>
    public interface IPersistableSettings
    {
        /// <summary>
        /// 
        /// </summary>
        string SettingId { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingId"></param>
        void AssignSettingId(string settingId);

        /// <summary>
        /// Assigns default values to the settings. 
        /// These values will be overriden with values that are read from the settings file.
        /// </summary>
        void HydrateWithDefaults();
    }
}
