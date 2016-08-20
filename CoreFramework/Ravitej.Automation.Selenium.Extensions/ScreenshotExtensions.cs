using System;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace OpenQA.Selenium
{
    /// <summary>
    /// Extension methods to help with screenshots
    /// </summary>
    public static class ScreenshotExtensions
    {
        /// <summary>
        /// Persists a screenshot to disk in a PNG format
        /// </summary>
        /// <param name="screenshot">The screenshot to save</param>
        /// <param name="saveToFilePathAndName">The file path and file name ** WITHOUT AN EXTENTION **</param>
        public static bool PersistScreenshot(this Screenshot screenshot, string saveToFilePathAndName)
        {
            if (screenshot != null)
            {
                try
                {
                    var fileInfo = new FileInfo(saveToFilePathAndName);
                    if (fileInfo.DirectoryName != null && !Directory.Exists(fileInfo.DirectoryName))
                    {
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    }
                    screenshot.SaveAsFile(string.Concat(saveToFilePathAndName, ".png"), ImageFormat.Png);
                    return true;
                }
                catch (Exception e)
                {
                    LogX.Error.Write("Could not save screenshot due to an error: {0}", e.ToString());
                }
            }

            return false;
        }
    }
}
