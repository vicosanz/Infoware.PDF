using Infoware.PDF.Helpers;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;

namespace Infoware.PDF
{
    public class Row
    {
        private IGenerator _generator;
        private Table _table;
        private double _x;
        private double _y;
        private double _height;
        private int _rowIndex;
        private int _cellIndex = -1;

        public Row(IGenerator generator, Table table, int rowIndex, double X, double Y, double height)
        {
            _generator = generator;
            _table = table;
            _x = X;
            _y = Y;
            _height = height;
            _rowIndex = rowIndex;
        }

        public void AddCell(string text, XParagraphAlignment alignment = XParagraphAlignment.Center, bool autoGrowHeight = false)
        {
            _cellIndex++;
            double width = _table.ColumnsWidth[_cellIndex];
            if (autoGrowHeight)
            {
                _generator.GetTextHeight(text, width - 4, out double textHeight);
                if (textHeight > _height)
                {
                    _height = textHeight;
                }
            }
            if (_table.DrawBorders)
            {
                _generator.Rectangle(new XRect(_x, _y, width, _height));
            }
            _generator.WriteInBox(text, alignment, _x + 1, _y + 1, width - 2, _height - 2);
            _x += width;
        }

        public void AddCell(decimal? valor)
        {
            _cellIndex++;
            double width = _table.ColumnsWidth[_cellIndex];
            if (_table.DrawBorders)
            {
                _generator.Rectangle(new XRect(_x, _y, width, _height));
            }
            _generator.WriteInBox(valor ?? 0, _x + 1, _y + 1, width - 2, _height - 2);
            _x += width;
        }

        public void AddCell(int? valor)
        {
            _cellIndex++;
            double width = _table.ColumnsWidth[_cellIndex];
            if (_table.DrawBorders)
            {
                _generator.Rectangle(new XRect(_x, _y, width, _height));
            }
            _generator.WriteInBox(valor ?? 0, _x + 1, _y + 1, width - 2, _height - 2);
            _x += width;
        }
    }
}