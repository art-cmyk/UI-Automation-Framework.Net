using System;
using System.Drawing;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Ravitej.Automation.Common.Utilities.Imaging;
using Ravitej.Automation.Common.Utilities;

namespace Ravitej.Automation.Common.Helpers
{
    /// <summary>
    /// Helper class to deal with the taking of screenshots
    /// </summary>
    public static class ScreenshotHelper
    {
        /// <summary>
        /// Takes a screenshot - if there is an alert, an error occurs
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="fullSavePath">Destination save path</param>
        /// <param name="imageOverlayText"></param>
        /// <param name="useGreenBrushColour">Should the brush colour be in red or the default green</param>
        public static void TakeScreenshot(IWebDriver driver, string fullSavePath, string imageOverlayText = "", bool useGreenBrushColour = false)
        {
            try
            {
                Screenshot screenshot = driver.TakeScreenshot();
                if (screenshot.PersistScreenshot(fullSavePath.SanitisePath()))
                {
                    if (!string.IsNullOrWhiteSpace(imageOverlayText))
                    {
                        ImagingUtilities.OverlayTextOntoImage(fullSavePath.SanitisePath(), imageOverlayText, true, !useGreenBrushColour);
                    }
                }
            }
            catch (Exception e)
            {
                LogX.Error.Write("An error occurred while taking screenshot. {0}", e.ToString());
            }
        }

        /// <summary>
        /// Takes a screenshot - if there is an alert, dismisses it first
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="fullSavePath">Destination save path</param>
        /// <param name="overlayText"></param>
        public static void ForceTakeScreenshot(IWebDriver driver, string fullSavePath, string overlayText)
        {
            try
            {
                string dialogMessage;
                var s = new StringBuilder();
                if (driver.IsAlertBoxDisplayed(out dialogMessage))
                {
                    driver.SwitchTo().Alert().Accept();
                    s.AppendLine(dialogMessage);
                }
                s.AppendLine(overlayText);
                TakeScreenshot(driver, fullSavePath, s.ToString());
            }
            catch (Exception e)
            {
                LogX.Error.Write("An error occurred while taking screenshot. {0}", e.ToString());
            }
        }

        /// <summary>
        /// Takes a screenshot and highlights the specified target element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="targetElement">The element to highlight</param>
        /// <param name="fullSavePath">Destination save path</param>
        /// <param name="highlightInRed">Should the brush colour be in red or the default yellow</param>
        public static void TakeScreenshotAndHighlightElement(IWebDriver driver, IWebElement targetElement, string fullSavePath, bool highlightInRed = false)
        {
            try
            {
                Screenshot screenshot = driver.TakeScreenshot();
                var coordinates = targetElement.GetRectangle();
                using (var memoryStream = new MemoryStream(screenshot.AsByteArray))
                {
                    using (var image = Image.FromStream(memoryStream))
                    {
                        using (var newImage = ImagingUtilities.HilightZone(image, coordinates, 5, highlightInRed))
                        {
                            ImagingUtilities.SaveImage(newImage, fullSavePath.SanitisePath());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogX.Error.Write("An error occurred while taking screenshot. {0}", e.ToString());
            }
        }

        /// <summary>
        /// Takes a screenshot and crops it to the specified target element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="targetElement">The element to crop to</param>
        /// <param name="fullSavePath">Destination save path</param>
        public static void TakeScreenshotAndCropToElement(IWebDriver driver, IWebElement targetElement, string fullSavePath)
        {
            try
            {
                Screenshot screenshot = driver.TakeScreenshot();

                using (var image = Image.FromStream(new MemoryStream(screenshot.AsByteArray)))
                {
                    var coords = new Rectangle(targetElement.Location, targetElement.Size);
                    using (var newImage = ImagingUtilities.CropToSelection(image, coords))
                    {
                        ImagingUtilities.SaveImage(newImage, fullSavePath.SanitisePath());
                    }
                }
            }
            catch (Exception e)
            {
                LogX.Error.Write("An error occurred while taking screenshot. {0}", e.ToString());
            }
        }
    }
}
