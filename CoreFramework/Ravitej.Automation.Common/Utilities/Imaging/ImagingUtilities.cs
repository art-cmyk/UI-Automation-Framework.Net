using Ravitej.Automation.Common.Exceptions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Ravitej.Automation.Common.Utilities.Imaging
{
    /// <summary>
    /// Utility methods for working with images
    /// </summary>
    public static class ImagingUtilities
    {
        public static void SaveImage(Image sourceImage, string fileName)
        {
            string targetSourceFileName = string.Concat(fileName, ".png");
            sourceImage.Save(targetSourceFileName, ImageFormat.Png);
        }

        public static Image OverlayTextOntoImage(Image sourceImage, string overlayText, bool useRedBrushColour = true)
        {
            var brushColour = Brushes.Red;

            if (!useRedBrushColour)
            {
                brushColour = Brushes.Green;
            }

            using (Graphics g = Graphics.FromImage(sourceImage))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                var drawBounds = new RectangleF(0, 0, sourceImage.Width, sourceImage.Height);
                g.DrawString(overlayText, new Font("Tahoma", 40), brushColour, drawBounds, StringFormat.GenericTypographic);
            }

            return sourceImage;
        }

        /// <summary>
        /// Overlay the passed in text onto an image
        /// </summary>
        /// <param name="sourceOutputFileName"></param>
        /// <param name="imageOverlayText"></param>
        /// <param name="removeSourceImage"></param>
        /// <param name="useRedBrushColour"></param>
        public static void OverlayTextOntoImage(string sourceOutputFileName, string imageOverlayText, bool removeSourceImage, bool useRedBrushColour = true)
        {
            var brushColour = Brushes.Red;

            if (!useRedBrushColour)
            {
                brushColour = Brushes.Green;
            }

            string fullSourceFileName = string.Concat(sourceOutputFileName, ".png");

            // if there is text to overlay on the image
            if (!string.IsNullOrWhiteSpace(imageOverlayText))
            {
                // get the previous image from disk
                using (Image image = Image.FromFile(fullSourceFileName))
                {
                    // get the new file name
                    string outputFileName = string.Concat(sourceOutputFileName, "_overlay.png");

                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        var drawBounds = new RectangleF(0, 0, image.Width, image.Height);
                        g.DrawString(imageOverlayText, new Font("Tahoma", 40), brushColour, drawBounds, StringFormat.GenericTypographic);
                        //g.Save();
                    }

                    image.Save(outputFileName, ImageFormat.Png);
                }

                if (removeSourceImage)
                {
                    try
                    {
                        // remove the original one as it's been superceeded by the overlay one.
                        File.Delete(fullSourceFileName);
                    }
                    catch (IOException)
                    {
                        // swallow, can't delete the original - not ideal, but not anything to fret about.
                    }
                }
            }
        }

        /// <summary>
        /// For the passed in image, resize it to the passed in new size
        /// </summary>
        /// <param name="imgOrig"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        public static Image ResizeImage(Image imgOrig, Size newSize)
        {
            var newBm = new Bitmap(newSize.Width, newSize.Height);
            using (var newGrapics = Graphics.FromImage(newBm))
            {
                newGrapics.CompositingQuality = CompositingQuality.HighSpeed;
                newGrapics.SmoothingMode = SmoothingMode.HighSpeed;
                newGrapics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                newGrapics.DrawImage(imgOrig, new Rectangle(0, 0, newSize.Width, newSize.Height));
            }

            return newBm;
        }

        /// <summary>
        /// For the given image, crop it to the size expected
        /// </summary>
        /// <param name="sourceImage">The image to crop from</param>
        /// <param name="targetSelection">The bit to leave behind</param>
        /// <returns></returns>
        public static Image CropToSelection(Image sourceImage, Rectangle targetSelection)
        {
            Bitmap bmpImage = new Bitmap(sourceImage);
            return bmpImage.Clone(targetSelection, bmpImage.PixelFormat);
        }

        /// <summary>
        /// For the given image, this draws a box around the target zone
        /// </summary>
        /// <param name="sourceImage">The image to draw a box on</param>
        /// <param name="targetZone">The area to illustrate</param>
        /// <param name="isRed">Is the box red or the default yellow?</param>
        /// <returns></returns>
        public static Image HilightZone(Image sourceImage, Rectangle targetZone, int brushSize = 5, bool useRedBrushColour = false)
        {
            var newBm = new Bitmap(sourceImage.Width, sourceImage.Height);
            using (var newGrapics = Graphics.FromImage(sourceImage))
            {
                newGrapics.CompositingQuality = CompositingQuality.HighSpeed;
                newGrapics.SmoothingMode = SmoothingMode.HighSpeed;
                newGrapics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Color penColour = useRedBrushColour ? Color.Red : Color.Yellow;
                Pen targetPen = new Pen(penColour, brushSize);
                newGrapics.DrawRectangle(targetPen, targetZone);
            }

            return sourceImage;
        }

        public static float PercentageDifference(Bitmap sourceImage, Bitmap targetImage, string differenceSavePath = "")
        {
            if (sourceImage.Size != targetImage.Size)
            {
                throw new ImageComparisonSizeDifferenceException();
            }

            using (Bitmap differences = new Bitmap(sourceImage.Width, sourceImage.Height))
            {
                float DiferentPixelCount = 0;
                int TotalPixels = sourceImage.Width * sourceImage.Height;

                for (int i = 0; i < sourceImage.Width; ++i)
                {
                    for (int j = 0; j < sourceImage.Height; ++j)
                    {
                        Color secondColor = targetImage.GetPixel(i, j);
                        Color firstColor = sourceImage.GetPixel(i, j);

                        if (firstColor != secondColor)
                        {
                            DiferentPixelCount++;
                            differences.SetPixel(i, j, Color.Red);
                        }
                        else
                        {
                            differences.SetPixel(i, j, Color.Transparent);
                        }
                    }
                }

                float percentage = (DiferentPixelCount / TotalPixels) * 100;

                if (!string.IsNullOrWhiteSpace(differenceSavePath))
                {
                    OverlayTextOntoImage(differences, $"{percentage}% Different", false);
                    SaveImage(differences, differenceSavePath);
                }

                return percentage;
            }
        }

        public static float PercentageDifference(string sourceImagePath, string targetImagePath, string differenceSavePath = "")
        {
            Bitmap sourceImage = new Bitmap(sourceImagePath);
            Bitmap targetImage = new Bitmap(targetImagePath);

            if (sourceImage.Size != targetImage.Size)
            {
                throw new ImageComparisonSizeDifferenceException(sourceImagePath, targetImagePath);
            }

            return PercentageDifference(sourceImage, targetImage, differenceSavePath);
        }
    }
}
