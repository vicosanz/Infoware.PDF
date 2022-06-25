using System.Collections.Generic;
using PdfSharpCore.Drawing.Layout;

namespace Infoware.PDF.Helpers
{
    public static class TableHelper
    {
        /// <summary>
        /// Create a new Table and set as current
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        /// <param name="columnsWidth">List of column widths</param>
        /// <returns>The generator</returns>
        public static IGenerator WithTable(this IGenerator generator, double X, double Y, List<double> columnsWidth, bool drawBorders = true, double defaultRowHeight = 10)
        {
            if (generator.Expression)
            {
                generator.CurrentTable = new Table(generator, X, Y, columnsWidth)
                {
                    DrawBorders = drawBorders,
                    DefaultRowHeight = defaultRowHeight
                };
            }
            return generator;
        }

        /// <summary>
        /// Create a new Row for the current Table
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="height">Row Height</param>
        /// <returns>The generator</returns>
        public static IGenerator AddRow(this IGenerator generator, double? height = null)
        {
            if (generator.Expression)
            {
                generator.CurrentTable.AddRow(height);
            }
            return generator;
        }

        /// <summary>
        /// Create a new Row for the current Row
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="text">Inside Text</param>
        /// <returns>The generator</returns>
        public static IGenerator AddCell(this IGenerator generator, string text, XParagraphAlignment alignment = XParagraphAlignment.Center, bool autoGrowHeight = false)
        {
            if (generator.Expression)
            {
                generator.CurrentTable.CurrentRow.AddCell(text, alignment, autoGrowHeight);
            }
            return generator;
        }

        /// <summary>
        /// Create a new Row for the current Row
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="valor">Numeric Text</param>
        /// <returns>The generator</returns>
        public static IGenerator AddCell(this IGenerator generator, decimal? valor)
        {
            if (generator.Expression)
            {
                generator.CurrentTable.CurrentRow.AddCell(valor);
            }
            return generator;
        }

        /// <summary>
        /// Create a new Row for the current Row
        /// </summary>
        /// <param name="generator">The generator</param>
        /// <param name="valor">Numeric Text</param>
        /// <returns>The generator</returns>
        public static IGenerator AddCell(this IGenerator generator, int? valor)
        {
            if (generator.Expression)
            {
                generator.CurrentTable.CurrentRow.AddCell(valor);
            }
            return generator;
        }

    }
}
