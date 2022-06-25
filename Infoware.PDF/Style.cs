using PdfSharpCore.Drawing;

namespace Infoware.PDF
{
    public class Style
    {
        public Style(XFont font, XSolidBrush brush)
        {
            Font = font;
            Brush = brush;
        }
        public XFont Font { get; }
        public XSolidBrush Brush { get; }
    }
}
