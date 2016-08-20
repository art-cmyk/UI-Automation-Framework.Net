using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Ravitej.Automation.Common.Utilities.Imaging
{
    /// <summary>
    /// The shape of the balloon to overlay on the image
    /// </summary>
    public enum BalloonShape
    {
        /// <summary>
        /// Render as an ellipse
        /// </summary>
        Ellipse,

        /// <summary>
        /// Render as a rectangle
        /// </summary>
        Rectangle
    }

    /// Based on code taken from:
    /// by Clayton Rumley, clayton@digifi.ca
    /// http://www.digifi.ca
    /// Anyone is free to use this code, but please credit me and let me know if you do
    public class SpeechBubble
    {
        private const float BorderWidth = 2.0f;

        private readonly Color _borderColor;

        private Rectangle _myBounds;

        /// <summary>
        /// Render a speech bubble using the passed in text
        /// </summary>
        /// <param name="textContent"></param>
        public SpeechBubble(string textContent = "Text Goes Here")
        {
            _borderColor = Color.Black;
            _myBounds = new Rectangle(0, 0, 200, 100);
            BubbleWidth = 20;
            FillColor = Color.White;
            TargetFont = new Font("Arial", 9.0f);
            TailRotation = 115.0f;
            TailVisible = true;
            Text = textContent;
            TextColor = Color.Red;
            Shape = BalloonShape.Ellipse;
            TailBaseWidth = 25;
            TailLength = 75;
            RecreatePath();
        }

        /// <summary>
        /// The bounding rectangle of the balloon, excluding the tail
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Rectangle Bounds
        {
            get
            {
                return _myBounds;
            }
            set
            {
                _myBounds = value;
            }
        }

        /// <summary>
        /// The size (depth) of the bubbles/bulges around the perimeter of the balloon. Specify 0 for a basic ellipse
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int BubbleSize
        {
            get;
            set;
        }

        /// <summary>
        /// The smoothness of the individual bulges/bubbles around the perimeter of the balloon
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public float BubbleSmoothness
        {
            get;
            set;
        }

        /// <summary>
        /// The width of each bulge/bubble around the perimeter of the balloon
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int BubbleWidth
        {
            get;
            set;
        }

        /// <summary>
        /// The fill (background) color of the balloon
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Color FillColor { get; set; }

        /// <summary>
        /// The font for the text in the balloon
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Font TargetFont
        {
            get;
            set;
        }

        /// <summary>
        /// get {s or sets the height of the balloon, excluding the tail
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Height
        {
            get
            {
                return _myBounds.Height;
            }
            set
            {
                //Dim changed As Boolean = value <> MyBounds.Height
                _myBounds.Height = value;
                //If changed Then
                //    RecreatePath()
                //    RaiseEvent RedrawRequired(Me, EventArgs.Empty)
                //}
            }
        }

        /// <summary>
        /// gets or sets the Left/X co-ordinate of the balloon, excluding the tail
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Left
        {
            get
            {
                return _myBounds.X;
            }
            set
            {
                //Dim changed As Boolean = MyBounds.X <> value
                _myBounds.X = value;
                //If changed Then
                //    RaiseEvent RedrawRequired(Me, EventArgs.Empty)
                //}
            }
        }

        /// <summary>
        /// The GraphicsPath that defines the bubble, excluding the tail
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public GraphicsPath Path { get; } = new GraphicsPath();

        /// <summary>
        /// The radius for the corner
        /// </summary>
        public int RectangleCornerRadius
        {
            get;
            set;
        }

        /// <summary>
        /// The overall shape of the balloon
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public BalloonShape Shape
        {
            get;
            set;
        }

        /// <summary>
        /// The width of the tail at its base
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int TailBaseWidth
        {
            get;
            set;
        }

        /// <summary>
        /// The length of the tail, as measured from the top of the balloon
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int TailLength
        {
            get;
            set;
        }

        /// <summary>
        /// The location (in degrees) of the tail around the bubble
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public float TailRotation
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether or not the tail is drawn
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool TailVisible
        {
            get;
            set;
        }

        /// <summary>
        /// The text in the balloon
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// The color of the text in the balloon
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Color TextColor { get; set; }

        /// <summary>
        /// gets or sets the Top/Y co-ordinate of the balloon, excluding the tail
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Top
        {
            get
            {
                return _myBounds.Top;
            }
            set
            {
                //Dim changed As Boolean = MyBounds.Y <> value

                _myBounds.Y = value;

                //If changed Then
                //    RaiseEvent RedrawRequired(Me, EventArgs.Empty)
                //}
            }
        }

        /// <summary>
        /// get {s or sets the width of the balloon, excluding the tail
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Width
        {
            get
            {
                return _myBounds.Width;
            }
            set
            {
                //Dim changed As Boolean = value <> MyBounds.Width

                _myBounds.Width = value;

                //If changed Then
                //    RecreatePath()
                //    RaiseEvent RedrawRequired(Me, EventArgs.Empty)
                //}
            }
        }

        /// <summary>
        /// Creates the GraphicsPath for the balloon
        /// </summary>
        /// <remarks></remarks>
        private void RecreatePath()
        {
            //'NOTE: To make creating the path easier, I assume the origin is (0, 0)
            //'when adding the points to the GraphicsPath. When it comes time to 
            //'actually draw the balloon, I'll call TranslateTransform on the
            //'Graphics object to shift the origin to the actual location as
            //'determined in the Bounds property.

            //Empty the path:
            Path.Reset();

            //If the BubbleSize is 0, we'll just create an ellipse:
            switch (Shape)
            {
                case (BalloonShape.Rectangle):
                    {
                        if (RectangleCornerRadius == 0)
                        {
                            //Do the easy one:
                            Path.AddRectangle(new Rectangle(0, 0, Width, Height));
                        }
                        else
                        {
                            //Round Rectangle code adapter from http://www.bobpowell.net/roundrects.htm
                            Path.AddLine(0 + RectangleCornerRadius, 0, 0 + Width - (RectangleCornerRadius * 2), 0);
                            Path.AddArc(0 + Width - (RectangleCornerRadius * 2), 0, RectangleCornerRadius * 2, RectangleCornerRadius * 2, 270, 90);
                            Path.AddLine(0 + Width, 0 + RectangleCornerRadius, 0 + Width, 0 + Height - (RectangleCornerRadius * 2));
                            Path.AddArc(0 + Width - (RectangleCornerRadius * 2), 0 + Height - (RectangleCornerRadius * 2), RectangleCornerRadius * 2, RectangleCornerRadius * 2, 0, 90);
                            Path.AddLine(0 + Width - (RectangleCornerRadius * 2), 0 + Height, 0 + RectangleCornerRadius, 0 + Height);
                            Path.AddArc(0, 0 + Height - (RectangleCornerRadius * 2), RectangleCornerRadius * 2, RectangleCornerRadius * 2, 90, 90);
                            Path.AddLine(0, 0 + Height - (RectangleCornerRadius * 2), 0, 0 + RectangleCornerRadius);
                            Path.AddArc(0, 0, RectangleCornerRadius * 2, RectangleCornerRadius * 2, 180, 90);
                        }
                        break;
                    }
                case (BalloonShape.Ellipse):
                    {
                        if (BubbleSize == 0)
                        {
                            Path.AddEllipse(0, 0, Width, Height);
                        }
                        else
                        {
                            int theta = 0;

                            //Do an angle sweep around the circle moving the BubbleWidth in each iteration:
                            for (theta = 0; theta <= (360 - BubbleWidth); theta += BubbleWidth)
                            {
                                var points = new Point[2];

                                double radianTheta = theta * Math.PI / 180;
                                double radianTheta2 = (theta + (BubbleWidth / 2)) * Math.PI / 180;
                                double radianTheta3 = (theta + BubbleWidth) * Math.PI / 180;

                                var x = (int)((Width / 2) + (Width / 2) * Math.Cos(radianTheta));
                                var y = (int)((Height / 2) + (Height / 2) * Math.Sin(radianTheta));

                                int xdelta = 0;
                                int ydelta = 0;

                                if (Math.Cos(radianTheta2) < 0 && Math.Sin(radianTheta2) < 0)
                                {
                                    //'Top-left:
                                    xdelta = -BubbleSize;
                                    ydelta = -BubbleSize;
                                }
                                else if (Math.Cos(radianTheta2) > 0 && Math.Sin(radianTheta2) > 0)
                                {
                                    //Bottom-Right, Good
                                    xdelta = BubbleSize;
                                    ydelta = BubbleSize;
                                }
                                else if (Math.Cos(radianTheta2) < 0 && Math.Sin(radianTheta2) > 0)
                                {
                                    //Bottom-Left, Good
                                    xdelta = -BubbleSize;
                                    ydelta = BubbleSize;
                                }
                                else if (Math.Cos(radianTheta2) > 0 && Math.Sin(radianTheta2) < 0)
                                {
                                    //Top-Right
                                    xdelta = BubbleSize;
                                    ydelta = -BubbleSize;
                                }

                                var x2 = (int)(((Width / 2) + (Width / 2) * Math.Cos(radianTheta2)) + xdelta);
                                var y2 = (int)(((Height / 2) + (Height / 2) * Math.Sin(radianTheta2)) + ydelta);

                                var x3 = (int)((Width / 2) + (Width / 2) * Math.Cos(radianTheta3));
                                var y3 = (int)((Height / 2) + (Height / 2) * Math.Sin(radianTheta3));

                                //Build the triangle between the start angle, the point away from the balloon 
                                //(as determined by the BubbleSize), and the sweep angle:

                                points[0] = new Point(x, y);
                                points[1] = new Point(x2, y2);
                                points[2] = new Point(x3, y3);

                                //The BubbleSmoothness value determines how curve-like the lines
                                //between the three points will be:
                                Path.AddCurve(points, BubbleSmoothness);
                            }
                        }
                        break;
                    }
            }

            //Finish off the path:
            Path.CloseAllFigures();
        }

        /// <summary>
        /// Draws the balloon into the provided graphics object:
        /// </summary>
        /// <param name="g"></param>
        /// <remarks></remarks>
        private void Draw(Graphics g)
        {
            GraphicsState gstate;
            var tail = new GraphicsPath();                          //The path for the tail
            var fillBrush = new SolidBrush(FillColor);              //The background color of the balloon and tail
            var textBrush = new SolidBrush(TextColor);              //The color of the text
            var borderPen = new Pen(_borderColor, BorderWidth);      //The border color & width
            var sf = new StringFormat();

            //Set the quality to high to make everything look nice and pretty:
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = SmoothingMode.HighQuality;

            //Create the tail//s path:
            //Note: To make drawing easier, I assume the tail is centered
            //around the center point of the balloon (from an origin of [0, 0])
            //and that it sticks straight up as far as TailLength.
            //When it comes time to actually draw the tail, I//ll
            //call TranslateTransform and RotateTransform to adjust the origin
            //and rotation to where we really want it:

            tail.AddLine(-TailBaseWidth, 0, TailBaseWidth, 0);
            tail.AddLine(TailBaseWidth, 0, 0, -(TailLength + (Height / 2)));

            tail.CloseFigure();

            //Here are the steps to drawing this balloon:
            //1. Draw the tail//s border, twice as thick as the balloon//s border
            //   When the balloon is filled it will cover up the tail border that
            //   is drawn under the balloon itself
            //2. Fill the balloon//s path using the background color
            //3. Draw the balloon//s border
            //4. Fill the tail//s path using the background color. This ensures
            //   that the outer border of the balloon (where it meets the tail
            //   is colored over with the background color, giving the illusion
            //   that the tail and the balloon are all one big happy object)

            //1. Draw the tail border first (if the border is visible):
            if (TailVisible)
            {
                //We double the pen//s size because we//re going to fill the
                //tail//s color overtop half of the border:
                var thickPen = new Pen(_borderColor, (float)(BorderWidth * 2.0));

                //Save the graphic state:
                gstate = g.Save();

                //Move to our tail//s origin (center of the balloon):
                float leftPos = ((float)Left + (Width / 2));
                g.TranslateTransform(leftPos, Top + (Height / 2));

                //Rotate the tail around its origin:
                g.RotateTransform(TailRotation);

                //Draw the border:
                g.DrawPath(thickPen, tail);

                //Restore the previous graphic state:
                g.Restore(gstate);
            }

            //Save the state again:
            gstate = g.Save();

            //Move to our balloon//s origin:
            g.TranslateTransform(Left, Top);

            //2. Fill the balloon//s path using the background brush:
            g.FillPath(fillBrush, Path);

            //3. Draw the balloon//s border using the border pen (if (the border is visible):
            g.DrawPath(borderPen, Path);

            //Restore the previous graphic state:
            g.Restore(gstate);

            //4. Fill the tail//s path using the background brush:
            if (TailVisible)
            {
                //Save the state yet again:
                gstate = g.Save();

                //Move to our tail//s origin (center of the balloon):
                g.TranslateTransform((Left + (Width / 2)), Top + (Height / 2));

                //Rotate the tail around its origin:
                g.RotateTransform(TailRotation);

                //Fill //er up:
                //   This will cover up half of the tail border (thus our need for doubling it above)
                //   and will cover up the balloon border where the balloon and the tail intersect
                g.FillPath(fillBrush, tail);

                //Restore the graphics state:
                g.Restore(gstate);
            }

            //Set our text alignment within the bounds of the balloon, excluding the tail
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            //Draw out our text using the font and text color brush:
            g.DrawString(Text, TargetFont, textBrush, Bounds, sf);

            //Cleaning up the mess:
            fillBrush.Dispose();
            textBrush.Dispose();
            borderPen.Dispose();
        }

        /// <summary>
        /// Render text over the image
        /// </summary>
        /// <param name="sourceOutputFileName"></param>
        /// <param name="removeSourceImage"></param>
        public void RenderText(string sourceOutputFileName, bool removeSourceImage)
        {
            string fullSourceFileName = string.Concat(sourceOutputFileName, ".png");

            // get the previous image from disk
            using (Image image = Image.FromFile(fullSourceFileName))
            {
                // get the new file name
                string outputFileName = string.Concat(sourceOutputFileName, "_bubble.png");

                using (Graphics g = Graphics.FromImage(image))
                {
                    RecreatePath();
                    Draw(g);
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
}
