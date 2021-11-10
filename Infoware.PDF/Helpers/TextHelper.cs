using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Linq;

namespace Infoware.PDF.Helpers
{
    public static class TextHelper
    {
        /// <summary>
        /// Set current style for following write operations
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="style">Style</param>
        /// <returns>The generator</returns>
        public static IGenerator WithStyle(this IGenerator generator, Style style)
        {
            generator.CurrentStyle = style;
            return generator;
        }

        public static IGenerator GetTextLines(this IGenerator generator, string text, double width, out int textLines)
        {
            var splitted = text.Split('\n').ToList();
            textLines = 0;
            foreach (var line in splitted)
            {
                textLines += GetLines(generator, line, width);
            }
            return generator;
        }

        private static int GetLines(IGenerator generator, string text, double width)
        {
            int textLines;
            var splitted = text.Split(' ').ToList();
            textLines = 0;

            var parts = 0;
            while (parts < splitted.Count)
            {
                var lineText = string.Join(' ', splitted.Take(parts + 1));
                if (generator.Draw.MeasureString(lineText, generator.CurrentStyle.Font).Width > width)
                {
                    splitted = splitted.Skip(parts).ToList();
                    textLines++;
                    parts = 0;
                }
                else
                {
                    parts++;
                }
            }
            textLines++;
            return textLines;
        }

        public static IGenerator GetTextHeight(this IGenerator generator, string text, double width, out double textHeight)
        {
            var height = generator.Draw.MeasureString("x", generator.CurrentStyle.Font).Height +2;
            generator.GetTextLines(text, width, out int textLines);
            textHeight = height * textLines;
            return generator;
        }

        /// <summary>
        /// Write String using the current Style
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="text">Text to write</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>The generator</returns>
        public static IGenerator Write(this IGenerator generator, string text, double x, double y)
        {
            if (generator.Expression)
            {
                generator.Draw.DrawString(text ?? "", generator.CurrentStyle.Font, generator.CurrentStyle.Brush, x, y);
            }
            return generator;
        }

        /// <summary>
        /// Write String using the current Style
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="text">Text to write</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Box width</param>
        /// <param name="height">Box height</param>
        /// <returns>The generator</returns>
        public static IGenerator WriteInBox(this IGenerator generator, string text, XParagraphAlignment alignment, double x, double y, double width, double height)
        {
            if (generator.Expression)
            {
                var formatter = new XTextFormatter(generator.Draw)
                {
                    Alignment = alignment
                };
                formatter.DrawString(text ?? "", generator.CurrentStyle.Font, generator.CurrentStyle.Brush,
                    new XRect(x, y, width, height));
            }
            return generator;
        }

        /// <summary>
        /// Write String using the current Style
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="valor">Number to write</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Box width</param>
        /// <param name="height">Box height</param>
        /// <returns>The generator</returns>
        public static IGenerator WriteInBox(this IGenerator generator, decimal valor, double x, double y, double width, double height)
        {
            if (generator.Expression)
            {
                var formatter = new XTextFormatter(generator.Draw)
                {
                    Alignment = XParagraphAlignment.Right
                };
                formatter.DrawString(valor.ToString("0.00"), generator.CurrentStyle.Font, generator.CurrentStyle.Brush,
                    new XRect(x, y, width, height));
            }
            return generator;
        }

        /// <summary>
        /// Write String using the current Style
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="valor">Number to write</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Box width</param>
        /// <param name="height">Box height</param>
        /// <returns>The generator</returns>
        public static IGenerator WriteInBox(this IGenerator generator, long valor, double x, double y, double width, double height)
        {
            if (generator.Expression)
            {
                var formatter = new XTextFormatter(generator.Draw)
                {
                    Alignment = XParagraphAlignment.Right
                };
                formatter.DrawString(valor.ToString(), generator.CurrentStyle.Font, generator.CurrentStyle.Brush,
                    new XRect(x, y, width, height));
            }
            return generator;
        }
    }
}
