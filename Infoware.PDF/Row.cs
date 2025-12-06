using Infoware.PDF.Helpers;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using System.Collections.Generic;

namespace Infoware.PDF;

public class Row(IGenerator generator, Table table, double X, double Y, double height, bool autoHeight)
{
    private int _cellIndex = -1;
    private readonly List<CellData> cells = [];

    public double Height => height;

    public void AddCell(string text)
    {
        AddCell(text, table.TextFormatter.Alignment);
    }

    public void AddCell(string text, XParagraphAlignment paragraphAlignment = XParagraphAlignment.Center)
    {
        _cellIndex++;
        double width = table.ColumnsWidth[_cellIndex];
        CellData cell = new()
        {
            Text = text,
            ParagraphAlignment = paragraphAlignment,
            VerticalAlignment = table.TextFormatter.VerticalAlignment,
            X = X,
            Width = width,
        };
        if (autoHeight)
        {
            generator.GetTextHeight(text, width - 4, out double textHeight);
            if (textHeight > Height)
            {
                height = textHeight;
            }
            cells.Add(cell);
        }
        else
        {
            WriteCell(cell, table.DrawBorders);
        }
        X += width;
    }

    public void AddCell(decimal? valor, string format = "0.00")
    {
        _cellIndex++;
        double width = table.ColumnsWidth[_cellIndex];
        string valorString = (valor ?? 0).ToString(format);
        CellData cell = new()
        {
            Text = valorString,
            ParagraphAlignment = XParagraphAlignment.Right,
            VerticalAlignment = table.TextFormatter.VerticalAlignment,
            X = X,
            Width = width,
        };
        if (autoHeight)
        {
            cells.Add(cell);
        }
        else
        {
            WriteCell(cell, table.DrawBorders);
        }
        X += width;
    }

    public void AddCell(int? valor)
    {
        _cellIndex++;
        double width = table.ColumnsWidth[_cellIndex];
        CellData cell = new()
        {
            Text = (valor ?? 0).ToString(),
            ParagraphAlignment = XParagraphAlignment.Right,
            VerticalAlignment = table.TextFormatter.VerticalAlignment,
            X = X,
            Width = width,
        };
        if (autoHeight)
        {
            cells.Add(cell);
        }
        else
        {
            WriteCell(cell, table.DrawBorders);
        }
        X += width;
    }

    public void DrawCells()
    {
        foreach(var cell in cells)
        {
            WriteCell(cell, table.DrawBorders);
        }
    }

    private IGenerator WriteCell(CellData cell, bool drawBorders)
    {
        if (drawBorders)
        {
            generator.Rectangle(new XRect(cell.X, Y, cell.Width, Height));
        }
        return generator.WriteInBox(cell.Text, cell.ParagraphAlignment, cell.VerticalAlignment, cell.X + 1, Y + 1, cell.Width - 2, Height - 2);
    }
}