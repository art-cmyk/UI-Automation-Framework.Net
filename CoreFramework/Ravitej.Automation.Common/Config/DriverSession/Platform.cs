namespace Ravitej.Automation.Common.Config.DriverSession
{
    /// <summary>
    /// Determines the platform to execute the suite of tests on.
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// Microsoft Windows
        /// </summary>
        Windows,

        /// <summary>
        /// Apple Mac OSX
        /// </summary>
        Mac,

        /// <summary>
        /// Linux
        /// </summary>
        Linux,

        /// <summary>
        /// Apple iOS (iPhone and iPad)
        /// </summary>
        iOS,

        /// <summary>
        /// Android
        /// </summary>
        Android,

        ///// <summary>
        ///// Not specified - used the desired capabilities to handle any target settings
        ///// </summary>
        //Unspecified
    }

    /// <summary>
    /// Determines the specific Windows OS to execute the suite of tests on.
    /// </summary>
    internal enum WindowsOs
    {
        /// <summary>
        /// Windows
        /// </summary>
        Windows7,

        /// <summary>
        /// Windows 8
        /// </summary>
        Windows8,

        /// <summary>
        /// Windows 8.1
        /// </summary>
        Windows81,

        /// <summary>
        /// Windows 10
        /// </summary>
        Windows10,

        /// <summary>
        /// Windows 10 Mobile
        /// </summary>
        Windows10Mobile,

        /// <summary>
        /// Windows XP
        /// </summary>
        WindowsXp
    }

    /// <summary>
    /// Determines the specific Windows OS to execute the suite of tests on.
    /// </summary>
    internal enum MacOs
    {
        /// <summary>
        /// OSX Mavericks
        /// </summary>
        Mavericks,

        /// <summary>
        /// OSX Snow Leopard
        /// </summary>
        SnowLeopard,

        /// <summary>
        /// OSX Lion
        /// </summary>
        Lion,

        /// <summary>
        /// OSX Mountain Lion
        /// </summary>
        MountainLion,

        /// <summary>
        /// OSX Yosemite
        /// </summary>
        Yosemite
    }
}