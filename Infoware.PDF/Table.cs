using System.Collections.Generic;

namespace Infoware.PDF
{
    public class Table
    {
        private readonly IGenerator _generator;
        private readonly double _x;

        public bool DrawBorders { get; init; } = true;
        public double DefaultRowHeight { get; init; } = 10;
        public List<double> ColumnsWidth { get; internal set; }

        private int _rowIndex = -1;

        public Row CurrentRow { get; set; }

        public Table(IGenerator generator, double x, double y, List<double> columnsWidth)
        {
            _generator = generator;
            _x = x;
            generator.PointerY = y;
            ColumnsWidth = columnsWidth;
        }

        public IGenerator AddRow(double? height)
        {
            var rowHeight = height ?? DefaultRowHeight;
            _generator.CheckEndOfPage(rowHeight);
            _rowIndex++;
            CurrentRow = new Row(_generator, this, _rowIndex, _x, _generator.PointerY, rowHeight, false);
            _generator.PointerY += rowHeight;
            return _generator;
        }

        public IGenerator AddRowAutoHeight()
        {
            CurrentRow = new Row(_generator, this, _rowIndex, _x, _generator.PointerY, DefaultRowHeight, true);
            return _generator;
        }

        public IGenerator DrawRowAutoHeight()
        {
            var rowHeight = CurrentRow.Height;
            _generator.CheckEndOfPage(rowHeight);
            _rowIndex++;
            CurrentRow.DrawCells();
            _generator.PointerY += rowHeight;
            return _generator;
        }

    }
}