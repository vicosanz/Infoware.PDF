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

    public void AddCell(string text, XParagraphAlignment alignment = XParagraphAlignment.Center)
    {
        _cellIndex++;
        double width = table.ColumnsWidth[_cellIndex];
        if (autoHeight)
        {
            generator.GetTextHeight(text, width - 4, out double textHeight);
            if (textHeight > Height)
            {
                height = textHeight;
            }
        }
        if (autoHeight)
        {
            cells.Add(new CellData()
            {
                Text = text,
                Alignment = alignment,
                X = X,
                Width = width,
            });
        }
        else
        {
            if (table.DrawBorders)
            {
                generator.Rectangle(new XRect(X, Y, width, Height));
            }
            generator.WriteInBox(text, alignment, X + 1, Y + 1, width - 2, Height - 2);
        }
        X += width;
    }

    public void AddCell(decimal? valor)
    {
        _cellIndex++;
        double width = table.ColumnsWidth[_cellIndex];
        if (autoHeight)
        {
            cells.Add(new CellData()
            {
                Text = (valor ?? 0).ToString("0.00"),
                Alignment = XParagraphAlignment.Right,
                X = X,
                Width = width,
            });
        }
        else
        {
            if (table.DrawBorders)
            {
                generator.Rectangle(new XRect(X, Y, width, Height));
            }
            generator.WriteInBox(valor ?? 0, X + 1, Y + 1, width - 2, Height - 2);
        }
        X += width;
    }

    public void AddCell(int? valor)
    {
        _cellIndex++;
        double width = table.ColumnsWidth[_cellIndex];
        if (autoHeight)
        {
            cells.Add(new CellData()
            {
                Text = (valor ?? 0).ToString(),
                Alignment = XParagraphAlignment.Right,
                X = X,
                Width = width,
            });
        }
        else
        {
            if (table.DrawBorders)
            {
                generator.Rectangle(new XRect(X, Y, width, Height));
            }
            generator.WriteInBox(valor ?? 0, X + 1, Y + 1, width - 2, Height - 2);
        }
        X += width;
    }

    public void DrawCells()
    {
        foreach(var cell in cells)
        {
            if (table.DrawBorders)
            {
                generator.Rectangle(new XRect(cell.X, generator.PointerY, cell.Width, Height));
            }
            generator.WriteInBox(cell.Text, cell.Alignment, cell.X + 1, generator.PointerY + 1, cell.Width - 2, Height - 2);
        }
    }
}