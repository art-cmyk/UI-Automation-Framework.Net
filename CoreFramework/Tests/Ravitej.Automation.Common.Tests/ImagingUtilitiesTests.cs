using Ravitej.Automation.Common.Exceptions;
using Ravitej.Automation.Common.Utilities.Imaging;
using NUnit.Framework;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Ravitej.Automation.Common.Tests
{
    [TestFixture]
    public class ImagingUtilitiesTests //: TestBaseUnitTests
    {
        [Test]
        public void CropImage_ValidateSizeIsAsExpected()
        {
            var newBm = new Bitmap(500, 500);
            var targetSize = new Rectangle(125, 125, 125, 125);
            using (var actual = ImagingUtilities.CropToSelection(newBm, targetSize))
            {
                Assert.AreEqual(125, actual.Height);
                Assert.AreEqual(125, actual.Width);
            }
        }

        [Test]
        public void CropImage_ValidateCroppedAreaIsAsExpected()
        {
            // create a new image
            using (var newBm = new Bitmap(500, 500))
            {
                // define the crop size
                var targetSize = new Rectangle(125, 125, 125, 125);

                // fill in the crop to a solid colour
                using (var newGrapics = Graphics.FromImage(newBm))
                {
                    newGrapics.CompositingQuality = CompositingQuality.HighSpeed;
                    newGrapics.SmoothingMode = SmoothingMode.HighSpeed;
                    newGrapics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    SolidBrush brush = new SolidBrush(Color.Red);
                    SolidBrush backgroundBrush = new SolidBrush(Color.Green);
                    newGrapics.FillRectangle(backgroundBrush, new Rectangle(0, 0, 500, 500));
                    newGrapics.FillRectangle(brush, targetSize);
                }

                //newBm.Save("D:\\Temp\\source.png", ImageFormat.Png);

                // performt he crop
                using (Bitmap actual = new Bitmap(ImagingUtilities.CropToSelection(newBm, targetSize)))
                {
                    Assert.AreEqual(125, actual.Height);
                    Assert.AreEqual(125, actual.Width);

                    //actual.Save("D:\\Temp\\sized.png", ImageFormat.Png);

                    // verify that all pixels are red
                    int DiferentPixelCount = 0;
                    Color expectedColor = Color.FromArgb(255, Color.Red);

                    for (int i = 0; i < actual.Width; ++i)
                    {
                        for (int j = 0; j < actual.Height; ++j)
                        {
                            Color pixelColor = actual.GetPixel(i, j);

                            if (pixelColor != expectedColor)
                            {
                                DiferentPixelCount++;
                            }
                        }
                    }

                    Assert.AreEqual(0, DiferentPixelCount, "Number of pixels that are not red is incorrect");

                }
            }
        }

        [Test]
        public void PercentageDifferences_SameImageExactmatch_NoSave()
        {
            using (var newBm = new Bitmap(500, 500))
            {
                // define the crop size
                var targetSize = new Rectangle(125, 125, 125, 125);

                // fill in the crop to a solid colour
                using (var newGrapics = Graphics.FromImage(newBm))
                {
                    newGrapics.CompositingQuality = CompositingQuality.HighSpeed;
                    newGrapics.SmoothingMode = SmoothingMode.HighSpeed;
                    newGrapics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    SolidBrush brush = new SolidBrush(Color.Red);
                    SolidBrush backgroundBrush = new SolidBrush(Color.Green);
                    newGrapics.FillRectangle(backgroundBrush, new Rectangle(0, 0, 500, 500));
                    newGrapics.FillRectangle(brush, targetSize);
                }

                var difference = ImagingUtilities.PercentageDifference(newBm, newBm, string.Empty);
                Assert.AreEqual(0, difference);
            }
        }

        [Test]
        public void PercentageDifferences_DifferentSize_ThrowsException()
        {
            using (var newBm = new Bitmap(500, 500))
            {
                using (var newBm2 = new Bitmap(200, 200))
                {
                    Assert.Throws<ImageComparisonSizeDifferenceException>(
                        () => ImagingUtilities.PercentageDifference(newBm, newBm2, string.Empty));
                }
            }
        }

        [Test]
        public void PercentageDifferences_DifferentImage_NoSave()
        {
            using (var newBm = new Bitmap(500, 500))
            {
                // define the crop size
                var targetSize = new Rectangle(125, 125, 125, 125);

                // fill in the crop to a solid colour
                using (var newGrapics = Graphics.FromImage(newBm))
                {
                    newGrapics.CompositingQuality = CompositingQuality.HighSpeed;
                    newGrapics.SmoothingMode = SmoothingMode.HighSpeed;
                    newGrapics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    SolidBrush brush = new SolidBrush(Color.Red);
                    SolidBrush backgroundBrush = new SolidBrush(Color.Green);
                    newGrapics.FillRectangle(backgroundBrush, new Rectangle(0, 0, 500, 500));
                    newGrapics.FillRectangle(brush, targetSize);
                }

                using (var newBm2 = new Bitmap(500, 500))
                {
                    // define the crop size
                    var targetSize2 = new Rectangle(250, 250, 250, 250);

                    // fill in the crop to a solid colour
                    using (var newGrapics = Graphics.FromImage(newBm2))
                    {
                        newGrapics.CompositingQuality = CompositingQuality.HighSpeed;
                        newGrapics.SmoothingMode = SmoothingMode.HighSpeed;
                        newGrapics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        SolidBrush brush = new SolidBrush(Color.Red);
                        SolidBrush backgroundBrush = new SolidBrush(Color.Green);
                        newGrapics.FillRectangle(backgroundBrush, new Rectangle(0, 0, 500, 500));
                        newGrapics.FillRectangle(brush, targetSize2);
                    }

                    var difference = ImagingUtilities.PercentageDifference(newBm, newBm2, string.Empty);
                    Assert.AreEqual(31.25, difference);
                }
            }
        }

        [Test]
        public void HilightZone_OnePxRed()
        {
            using (var newBm = new Bitmap(500, 500))
            {
                // define the crop size
                var targetSize = new Rectangle(125, 125, 125, 125);

                // fill in the crop to a solid colour
                using (var newGrapics = Graphics.FromImage(newBm))
                {
                    newGrapics.CompositingQuality = CompositingQuality.HighSpeed;
                    newGrapics.SmoothingMode = SmoothingMode.HighSpeed;
                    newGrapics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    SolidBrush brush = new SolidBrush(Color.Blue);
                    SolidBrush backgroundBrush = new SolidBrush(Color.Green);
                    newGrapics.FillRectangle(backgroundBrush, new Rectangle(0, 0, 500, 500));
                    newGrapics.FillRectangle(brush, targetSize);
                }

                //newBm.Save("D:\\Temp\\hilightedRed_Pre.png", ImageFormat.Png);

                using (var newImage = new Bitmap(ImagingUtilities.HilightZone(newBm, targetSize, 1, true)))
                {
                    int incorrectPixels = 0;
                    Color expectedColor = Color.FromArgb(255, Color.Red);

                    //newImage.Save("D:\\Temp\\hilightedRed_post.png", ImageFormat.Png);

                    // check each pixel along the top of the box
                    for (var i = 0; i < 125; i++)
                    {
                        Color pixelColor = newImage.GetPixel(125, 125 + i);

                        if (pixelColor != expectedColor)
                        {
                            incorrectPixels++;
                        }
                    }

                    // check each pixel along the bottom of the box
                    for (var i = 0; i < 125; i++)
                    {
                        Color pixelColor = newImage.GetPixel(250, 125 + i);

                        if (pixelColor != expectedColor)
                        {
                            incorrectPixels++;
                        }
                    }

                    // check each pixel along the left edge
                    for (var i = 0; i < 125; i++)
                    {
                        Color pixelColor = newImage.GetPixel(125, 125 + i);

                        if (pixelColor != expectedColor)
                        {
                            incorrectPixels++;
                        }
                    }

                    // check each pixel along the right edge
                    for (var i = 0; i < 125; i++)
                    {
                        Color pixelColor = newImage.GetPixel(250, 125 + i);

                        if (pixelColor != expectedColor)
                        {
                            incorrectPixels++;
                        }
                    }


                    Assert.AreEqual(0, incorrectPixels, "Number of pixels that are red is incorrect");
                }
            }
        }
    }
}
