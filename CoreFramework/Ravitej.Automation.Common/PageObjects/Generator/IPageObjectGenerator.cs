namespace Ravitej.Automation.Common.PageObjects.Generator
{
    /// <summary>
    /// Interface defining an object that can generate a page object
    /// </summary>
    public interface IPageObjectGenerator
    {
        /// <summary>
        /// Generate a page object
        /// </summary>
        /// <param name="targetFrameId"></param>
        void GeneratePageObject(string targetFrameId);
    }
}
