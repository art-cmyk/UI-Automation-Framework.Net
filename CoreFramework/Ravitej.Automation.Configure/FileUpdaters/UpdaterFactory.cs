using System;
using System.Linq;
using System.Reflection;
using Ravitej.Automation.Common.Config.SettingsUpdater;

namespace Ravitej.Automation.Configure.FileUpdaters
{
    internal static class UpdaterFactory
    {
        internal static ISettingsUpdater GetUpdater(string assemblyName)
        {
            Assembly assembly;
            assemblyName = assemblyName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? assemblyName : string.Concat(assemblyName, ".dll");
            try
            {
                assembly = Assembly.LoadFrom(assemblyName);
            }
            catch
            {
                throw new Exception($"Unable to load the given assembly: {assemblyName}");
            }

            var results = from type in assembly.GetTypes()
                          where typeof(ISettingsUpdater).IsAssignableFrom(type)
                          select type;
            results = results.ToList();

            if (!results.Any())
            {
                throw new Exception("There are no ISettingsUpdaters in the specified assembly");
            }
            if (results.Count() > 1)
            {
                throw new Exception("Please ensure that there is only a single implementation of ISettingsUpdaters in the specified assembly");
            }
            return (ISettingsUpdater)Activator.CreateInstance(results.First());
        }
    }
}