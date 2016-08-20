using System;
using Ravitej.Automation.Common.Config;
using Newtonsoft.Json;

namespace Ravitej.Automation.Common.Utilities.Persistance
{
    internal static class SettingsPersistance
    {
        public static string Serialize(IPersistableSettings settings)
        {
            if (settings == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(settings, Formatting.Indented);
        }

        public static TPersistableSettings Deserialize<TPersistableSettings>(string inputString) where TPersistableSettings : IPersistableSettings
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                return default(TPersistableSettings);
            }

            return JsonConvert.DeserializeObject<TPersistableSettings>(inputString);
        }
    }
}
