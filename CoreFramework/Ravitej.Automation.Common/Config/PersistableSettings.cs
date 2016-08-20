using System.Runtime.Serialization;

namespace Ravitej.Automation.Common.Config
{
    /// <summary>
    /// Provides the <b>abstract</b> base class for a settings object that can be persisted.
    /// </summary>
    public abstract class PersistableSettings : IPersistableSettings
    {
        [IgnoreDataMember]
        public string SettingId { get; private set; }

        public void AssignSettingId(string settingId)
        {
            SettingId = settingId;
        }

        public abstract void HydrateWithDefaults();
    }
}