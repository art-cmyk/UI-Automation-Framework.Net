namespace Ravitej.Automation.Common.Config.DriverSession
{
    /// <summary>
    /// Determines the browser to execute a suite of tests in.
    /// </summary>
    public enum Browser
    {
        /// <summary>
        /// Mozilla Firefox
        /// </summary>
        Firefox,

        /// <summary>
        /// Google Chrome
        /// </summary>
        Chrome,

        /// <summary>
        /// Microsoft Internet Explorer
        /// </summary>
        InternetExplorer,

        /// <summary>
        /// Apple Safari
        /// </summary>
        Safari,

        /// <summary>
        /// Opera
        /// </summary>
        Opera,

        /// <summary>
        /// Microsoft Edge
        /// </summary>
        Edge,

        /// <summary>
        /// Not specified - used the desired capabilities to handle any target settings
        /// </summary>
        Unspecified
    }
}
