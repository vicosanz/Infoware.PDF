using PdfSharpCore.Drawing.Layout;

namespace Infoware.PDF
{
    public class CellData
    {
        public double X { get; internal set; }
        public double Y { get; internal set; }
        public double Width { get; internal set; }
        public string Text { get; internal set; }
        public XParagraphAlignment Alignment { get; internal set; }
    }
}