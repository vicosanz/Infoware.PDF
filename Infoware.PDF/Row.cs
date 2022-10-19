using Infoware.PDF.Helpers;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Infoware.PDF
{
    public class Row
    {
        private readonly IGenerator _generator;
        private readonly Table _table;
        private double _x;
        private readonly double _y;
        private double _height;
        private readonly int _rowIndex;
        private int _cellIndex = -1;
        private bool _autoHeight = false;

        private List<CellData> cells = new();

        public double Height => _height;

        public Row(IGenerator generator, Table table, int rowIndex, double X, double Y, double height, bool autoHeight)
        {
            _generator = generator;
            _table = table;
            _x = X;
            _y = Y;
            _height = height;
            _rowIndex = rowIndex;
            _autoHeight = autoHeight;
        }

        public void AddCell(string text, XParagraphAlignment alignment = XParagraphAlignment.Center)
        {
            _cellIndex++;
            double width = _table.ColumnsWidth[_cellIndex];
            if (_autoHeight)
            {
                _generator.GetTextHeight(text, width - 4, out double textHeight);
                if (textHeight > Height)
                {
                    _height = textHeight;
                }
            }
            if (_autoHeight)
            {
                cells.Add(new CellData()
                {
                    Text = text,
                    Alignment = alignment,
                    X = _x,
                    Y = _y,
                    Width = width,
                });
            }
            else
            {
                if (_table.DrawBorders)
                {
                    _generator.Rectangle(new XRect(_x, _y, width, Height));
                }
                _generator.WriteInBox(text, alignment, _x + 1, _y + 1, width - 2, Height - 2);
            }
            _x += width;
        }

        public void AddCell(decimal? valor)
        {
            _cellIndex++;
            double width = _table.ColumnsWidth[_cellIndex];
            if (_autoHeight)
            {
                cells.Add(new CellData()
                {
                    Text = (valor ?? 0).ToString("0.00"),
                    Alignment = XParagraphAlignment.Right,
                    X = _x,
                    Y = _y,
                    Width = width,
                });
            }
            else
            {
                if (_table.DrawBorders)
                {
                    _generator.Rectangle(new XRect(_x, _y, width, Height));
                }
                _generator.WriteInBox(valor ?? 0, _x + 1, _y + 1, width - 2, Height - 2);
            }
            _x += width;
        }

        public void AddCell(int? valor)
        {
            _cellIndex++;
            double width = _table.ColumnsWidth[_cellIndex];
            if (_autoHeight)
            {
                cells.Add(new CellData()
                {
                    Text = (valor ?? 0).ToString(),
                    Alignment = XParagraphAlignment.Right,
                    X = _x,
                    Y = _y,
                    Width = width,
                });
            }
            else
            {
                if (_table.DrawBorders)
                {
                    _generator.Rectangle(new XRect(_x, _y, width, Height));
                }
                _generator.WriteInBox(valor ?? 0, _x + 1, _y + 1, width - 2, Height - 2);
            }
            _x += width;
        }

        public void DrawCells()
        {
            foreach(var cell in cells)
            {
                if (_table.DrawBorders)
                {
                    _generator.Rectangle(new XRect(cell.X, cell.Y, cell.Width, Height));
                }
                _generator.WriteInBox(cell.Text, cell.Alignment, cell.X + 1, cell.Y + 1, cell.Width - 2, Height - 2);
            }
        }
    }
}