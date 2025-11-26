using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharpCore.Drawing;

namespace Infoware.PDF.Barcodes;

internal record BarCodeRenderInfo2(XGraphics Gfx, XBrush Brush, XFont Font, XPoint Position)
{
    public XGraphics Gfx = Gfx;

    public XBrush Brush = Brush;

    public XFont Font = Font;

    public XPoint Position = Position;

    //public double BarHeight;

    public XPoint CurrPos;

    public int CurrPosInString;

    public double ThinBarWidth;
}
