using System;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace Infoware.PDF;

public class GeneratorPage : ICloneable, IDisposable, IEquatable<GeneratorPage>
{
    public Guid Id { get; set; }
    public PdfPage Page { get; private set; }

    public double MarginTop { get; init; }
    public double MarginBottom { get; init; }

    public XGraphics CurrentGraphics { get; internal set; }
    public double PointY { get; set; } = 10;

    internal GeneratorPage(Guid id, double marginTop, double marginBottom)
    {
        Id = id;
        MarginTop = marginTop;
        MarginBottom = marginBottom;
    }

    public GeneratorPage(PdfPage page, double marginTop, double marginBottom)
    {
        Id = Guid.NewGuid();
        Page = page;
        MarginTop = marginTop;
        MarginBottom = marginBottom;
        CurrentGraphics = XGraphics.FromPdfPage(Page);
    }

    public void ResetPointY()
    {
        PointY = MarginTop;
    }

    public GeneratorPage Clone()
    {
        return new GeneratorPage(Id, MarginTop, MarginBottom)
        {
            Page = Page,
            CurrentGraphics = CurrentGraphics,
            PointY = PointY
        };
    }

    object ICloneable.Clone()
    {
        return Clone();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            CurrentGraphics?.Dispose();
        }
    }

    public bool Equals(GeneratorPage other)
    {
        return other?.Id == Id;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as GeneratorPage);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
