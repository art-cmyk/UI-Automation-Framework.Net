namespace Ravitej.Automation.Common.Config.SuiteSettings
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILaunchPageHandler
    {
        /// <summary>
        /// For the given page Id, return the Url using any specific logic as needed by the page handler
        /// </summary>
        /// <param name="targetPageId"></param>
        /// <returns></returns>
        string GetLaunchUrl(int targetPageId);
    }
}
