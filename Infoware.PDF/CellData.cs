using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Drawing.Layout.enums;

namespace Infoware.PDF;

public record CellData
{
    public double X { get; internal set; }
    public double Width { get; internal set; }
    public string Text { get; internal set; }
    public XParagraphAlignment ParagraphAlignment { get; internal set; }
    public XVerticalAlignment VerticalAlignment { get; internal set; } 
}