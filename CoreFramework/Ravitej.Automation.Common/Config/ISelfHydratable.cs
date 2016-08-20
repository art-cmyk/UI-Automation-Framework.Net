namespace Ravitej.Automation.Common.Config
{
    /// <summary>
    /// A child object that will allow itself to be created, needed because interfaces cannot have the new() constraint.
    /// </summary>
    public interface ISelfHydratable<out T> where T : ISelfHydratableSetting
    {
        /// <summary>
        /// Creates a new instance of itself that is used when rehydrating.
        /// </summary>
        /// <returns></returns>
        T SelfHydrate();
    }
}