using System.Collections.Generic;
using PdfSharp.Drawing;

namespace Infoware.PDF
{
    public interface IGenerator
    {
        public void CheckEndOfPage(double alto);
        public bool Expression { get; set; }
        public void AddPage();
        public List<GeneratorPage> Pages { get; }
        public GeneratorPage CurrentPage { get; }
        public XPen CurrentPen { get; set; }
        public Style CurrentStyle { get; set; }
        public XGraphics Draw { get; }
        public Table CurrentTable { get; set; }
        public double PointerY { get; set; }
        public GeneratorPage PagePointer { get; set; }
        double MarginTop { get; set; }
        double MarginBottom { get; set; }
    }
}
