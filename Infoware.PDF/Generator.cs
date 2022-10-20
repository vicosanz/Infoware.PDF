using System;
using System.Collections.Generic;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace Infoware.PDF
{
    public class Generator : IGenerator, IDisposable
    {
        private readonly PdfDocument _document;
        private readonly List<GeneratorPage> _pages = new();
        private GeneratorPage _currentPage = null;
        private Style _currentStyle = new(new("Verdana", 7, XFontStyle.Regular), XBrushes.Black);
        private XPen _currentPen = new(XColors.Black, 1);
        private Table _currentTable;
        private bool _expression = true;
        private double _marginTop = 20;
        private double _marginBottom = 20;

        public Style CurrentStyle { get => _currentStyle; set => _currentStyle = value; }
        public Table CurrentTable { get => _currentTable; set => _currentTable = value; }
        public XGraphics Draw { get => _currentPage.CurrentGraphics; }
        public bool Expression { get => _expression; set => _expression = value; }
        public double PointerY { get => _currentPage.PointY; set => _currentPage.PointY = value; }
        public GeneratorPage PagePointer { get => _currentPage; set => _currentPage = value; }
        public double MarginTop { get => _marginTop; set => _marginTop = value; }
        public double MarginBottom { get => _marginBottom; set => _marginBottom = value; }
        public XPen CurrentPen { get => _currentPen; set => _currentPen = value; }

        public GeneratorPage CurrentPage => _currentPage;

        public List<GeneratorPage> Pages => _pages;

        public Generator(PdfDocument document)
        {
            _document = document;
        }

        /// <summary>
        /// Create a new instance of Generator
        /// </summary>
        /// <param name="document">PdfDocument</param>
        /// <returns>The new Generator object</returns>
        public static Generator Instance(PdfDocument document)
        {
            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return new Generator(document);
        }

        public void AddPage()
        {
            bool needNewPage = true;
            if (_currentPage != null)
            {
                var pageindex = _pages.FindIndex(x => x.Equals(_currentPage));
                if (pageindex < _pages.Count - 1)
                {
                    needNewPage = false;
                    _currentPage = _pages[pageindex + 1];
                    _currentPage.ResetPointY();
                }
            }
            if (needNewPage)
            {
                _currentPage = new GeneratorPage(_document.AddPage(), _marginTop, _marginBottom);
                _pages.Add(_currentPage);
            }
        }

        public void CheckEndOfPage(double heightNewElement)
        {
            if (_currentPage.PointY + heightNewElement > _currentPage.Page.Height - _marginBottom)
            {
                AddPage();
                _currentPage.ResetPointY();
            }
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
                if (_pages != null)
                {
                    foreach (var page in _pages)
                    {
                        page?.Dispose();
                    }
                }

            }
        }
    }
}
