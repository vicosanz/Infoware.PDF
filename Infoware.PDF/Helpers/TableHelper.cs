using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Drawing.Layout.enums;
using System.Collections.Generic;

namespace Infoware.PDF.Helpers;

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
    public static IGenerator WithTable(this IGenerator generator, double X, double Y, List<double> columnsWidth, bool drawBorders = true, double defaultRowHeight = 10,
        XParagraphAlignment paragraphAlignment = XParagraphAlignment.Center, XVerticalAlignment verticalAlignment = XVerticalAlignment.Top)
    {
        if (generator.Expression)
        {
            generator.CurrentTable = new Table(generator, X, Y, columnsWidth, paragraphAlignment, verticalAlignment)
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
    public static IGenerator WithFormat(this IGenerator generator, XParagraphAlignment paragraphAlignment, XVerticalAlignment verticalAlignment)
    {
        if (generator.Expression)
        {
            generator.CurrentTable.WithFormat(paragraphAlignment, verticalAlignment);
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
    /// Create a new Row for the current Table with Auto Height
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <returns>The generator</returns>
    public static IGenerator AddRowAutoHeight(this IGenerator generator)
    {
        if (generator.Expression)
        {
            generator.CurrentTable.AddRowAutoHeight();
        }
        return generator;
    }

    /// <summary>
    /// Draw a new Row for the current Table with Auto Height
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <returns>The generator</returns>
    public static IGenerator DrawRowAutoHeight(this IGenerator generator)
    {
        if (generator.Expression)
        {
            generator.CurrentTable.DrawRowAutoHeight();
        }
        return generator;
    }

    /// <summary>
    /// Create a new Row for the current Row
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <param name="text">Inside Text</param>
    /// <returns>The generator</returns>
    public static IGenerator AddCell(this IGenerator generator, string text)
    {
        if (generator.Expression)
        {
            generator.CurrentTable.CurrentRow.AddCell(text);
        }
        return generator;
    }

    /// <summary>
    /// Create a new Row for the current Row
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <param name="text">Inside Text</param>
    /// <returns>The generator</returns>
    public static IGenerator AddCell(this IGenerator generator, string text, XParagraphAlignment alignment = XParagraphAlignment.Center)
    {
        if (generator.Expression)
        {
            generator.CurrentTable.CurrentRow.AddCell(text, alignment);
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