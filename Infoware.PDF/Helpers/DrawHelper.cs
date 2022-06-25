using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.BarCodes;
using System.IO;

namespace Infoware.PDF.Helpers
{
    public static class DrawHelper
    {
        /// <summary>
        /// Set current Pen for following draws
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="pen">Pen</param>
        /// <returns>The generator</returns>
        public static IGenerator WithPen(this IGenerator generator, XPen pen)
        {
            generator.CurrentPen = pen;
            return generator;
        }

        /// <summary>
        /// Draw a RoundedRectangle using current Pen
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="xRect">Rectangle Coordinates</param>
        /// <returns>The generator</returns>
        public static IGenerator RoundedRectangle(this IGenerator generator, XRect xRect)
        {
            if (generator.Expression)
            {
                generator.Draw.DrawRoundedRectangle(generator.CurrentPen, xRect, new XSize(10, 10));
            }
            return generator;
        }

        /// <summary>
        /// Draw a Rectangle using current Pen
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="xRect">Rectangle Coordinates</param>
        /// <returns>The generator</returns>
        public static IGenerator Rectangle(this IGenerator generator, XRect xRect)
        {
            if (generator.Expression)
            {
                generator.Draw.DrawRectangle(generator.CurrentPen, xRect);
            }
            return generator;
        }

        public static IGenerator RectangleBegin(this IGenerator generator, double X, double Y, double width, out Rectangle rectangle)
        {
            if (generator.Expression)
            {
                rectangle = new(generator.PagePointer.Clone(), X, Y, width);
            }
            else
            {
                rectangle = null;
            }
            return generator;
        }

        public static IGenerator RectangleEnd(this IGenerator generator, Rectangle rectangle, double paddingBottom)
        {
            if (generator.Expression && rectangle != null)
            {
                generator.PointerY += paddingBottom;
                if (rectangle.GeneratorPage.Equals(generator.CurrentPage))
                {
                    return generator.Rectangle(
                        new XRect(rectangle.X, rectangle.Y,
                        rectangle.Width, generator.PointerY - rectangle.Y));
                }
                else
                {
                    var beginDraW = false;
                    foreach(var page in generator.Pages)
                    {
                        if (page.Equals(rectangle.GeneratorPage))
                        {
                            beginDraW = true;
                            page.CurrentGraphics.DrawLine(generator.CurrentPen, 
                                rectangle.X, rectangle.Y, rectangle.Width + rectangle.X, rectangle.Y);
                            page.CurrentGraphics.DrawLine(generator.CurrentPen,
                                rectangle.X, rectangle.Y, rectangle.X, page.Page.Height - page.MarginBottom);
                            page.CurrentGraphics.DrawLine(generator.CurrentPen,
                                rectangle.Width + rectangle.X, rectangle.Y, rectangle.Width + rectangle.X, page.Page.Height - page.MarginBottom);

                        }
                        else if (page.Equals(generator.CurrentPage))
                        {
                            page.CurrentGraphics.DrawLine(generator.CurrentPen,
                                rectangle.X, page.MarginTop, rectangle.X, generator.PointerY);
                            page.CurrentGraphics.DrawLine(generator.CurrentPen,
                                rectangle.Width + rectangle.X, page.MarginTop, rectangle.Width + rectangle.X, generator.PointerY);
                            page.CurrentGraphics.DrawLine(generator.CurrentPen,
                                rectangle.X, generator.PointerY, rectangle.Width + rectangle.X, generator.PointerY);
                            break;
                        }
                        else if (beginDraW)
                        {
                            page.CurrentGraphics.DrawLine(generator.CurrentPen,
                                rectangle.X, page.MarginTop, rectangle.X, page.Page.Height - page.MarginBottom);
                            page.CurrentGraphics.DrawLine(generator.CurrentPen,
                                rectangle.Width + rectangle.X, page.MarginTop, rectangle.Width + rectangle.X, page.Page.Height - page.MarginBottom);
                        }
                    }
                }
            }
            return generator;
        }

        /// <summary>
        /// Draw a BarCode39
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="text">Text to code</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        /// <param name="xSize">Size</param>
        /// <returns>The generator</returns>
        public static IGenerator WriteBarCode39(this IGenerator generator, string text, double X, double Y, XSize xSize)
        {
            if (generator.Expression)
            {
                Code3of9Standard barcode = new(text, xSize)
                {
                    StartChar = '*',
                    EndChar = '*'
                };
                generator.Draw.DrawBarCode(barcode, XBrushes.Black, new XPoint(X, Y));
            }
            return generator;
        }

        /// <summary>
        /// Draw an Image from file
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="imageFile">Image File</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        /// <param name="xSize">Size</param>
        /// <param name="stretch">True if the image fill the rectangle, False if the image must respect ratio</param>
        /// <returns>The generator</returns>
        public static IGenerator DrawImage(this IGenerator generator, string imageFile, double X, double Y, XSize xSize, bool stretch)
        {
            if (generator.Expression)
            {
                var image = XImage.FromFile(imageFile);
                if (!stretch)
                {
                    //convert pixel to point
                    double width = image.PixelWidth * 72 / image.HorizontalResolution;
                    double height = image.PixelHeight * 72 / image.HorizontalResolution;

                    double ratioWidth = xSize.Width / width;
                    double ratioHeight = xSize.Height / height;
                    double ratio = ratioWidth < ratioHeight ? ratioWidth : ratioHeight;
                    xSize.Width = width * ratio;
                    xSize.Height = height * ratio;
                }

                generator.Draw.DrawImage(image, X, Y, xSize.Width, xSize.Height);
            }
            return generator;
        }

        /// <summary>
        /// Draw an Image from file
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="imageStream">Image Stream</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        /// <param name="xSize">Size</param>
        /// <param name="stretch">True if the image fill the rectangle, False if the image must respect ratio</param>
        /// <returns>The generator</returns>
        public static IGenerator DrawImage(this IGenerator generator, Stream imageStream, double X, double Y, XSize xSize, bool stretch)
        {
            if (generator.Expression)
            {
                var image = XImage.FromStream(() => imageStream);
                if (!stretch)
                {
                    //convert pixel to point
                    double width = image.PixelWidth * 72 / image.HorizontalResolution;
                    double height = image.PixelHeight * 72 / image.HorizontalResolution;

                    double ratioWidth = xSize.Width / width;
                    double ratioHeight = xSize.Height / height;
                    double ratio = ratioWidth < ratioHeight ? ratioWidth : ratioHeight;
                    xSize.Width = width * ratio;
                    xSize.Height = height * ratio;
                }

                generator.Draw.DrawImage(image, X, Y, xSize.Width, xSize.Height);
            }
            return generator;
        }
    }
}
