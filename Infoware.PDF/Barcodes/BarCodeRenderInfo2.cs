using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharpCore.Drawing;

namespace Infoware.PDF.Barcodes
{
    internal class BarCodeRenderInfo2
    {
        public XGraphics Gfx;

        public XBrush Brush;

        public XFont Font;

        public XPoint Position;

        public double BarHeight;

        public XPoint CurrPos;

        public int CurrPosInString;

        public double ThinBarWidth;

        public BarCodeRenderInfo2(XGraphics gfx, XBrush brush, XFont font, XPoint position)
        {
            Gfx = gfx;
            Brush = brush;
            Font = font;
            Position = position;
        }
    }
}
