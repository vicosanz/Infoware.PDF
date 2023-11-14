using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharpCore.Drawing.BarCodes;
using PdfSharpCore.Drawing;

namespace Infoware.PDF.Barcodes
{
    public abstract class BarCode2 : CodeBase
    {
        private bool _turboBit;

        public virtual double WideNarrowRatio
        {
            get
            {
                return 0.0;
            }
            set
            {
            }
        }

        public TextLocation TextLocation { get; set; }

        public int DataLength { get; set; }

        public char StartChar { get; set; }

        public char EndChar { get; set; }

        public virtual bool TurboBit
        {
            get
            {
                return _turboBit;
            }
            set
            {
                _turboBit = value;
            }
        }

        public BarCode2(string text, XSize size, CodeDirection direction)
            : base(text, size, direction)
        {
            Text = text;
            Size = size;
            Direction = direction;
        }

        public static BarCode2 FromType(CodeType2 type, string text, XSize size, CodeDirection direction)
        {
            return type switch
            {
                CodeType2.Code128 => new Code128(text, size, direction),
                _ => throw new InvalidEnumArgumentException("type", (int)type, typeof(CodeType)),
            };
        }

        public static BarCode2 FromType(CodeType2 type, string text, XSize size)
        {
            return FromType(type, text, size, CodeDirection.LeftToRight);
        }

        public static BarCode2 FromType(CodeType2 type, string text)
        {
            return FromType(type, text, XSize.Empty, CodeDirection.LeftToRight);
        }

        public static BarCode2 FromType(CodeType2 type)
        {
            return FromType(type, string.Empty, XSize.Empty, CodeDirection.LeftToRight);
        }

        internal virtual void InitRendering(BarCodeRenderInfo2 info)
        {
            if (base.Text == null)
            {
                throw new InvalidOperationException("A text must be set before rendering the bar code.");
            }

            if (base.Size.IsEmpty)
            {
                throw new InvalidOperationException("A non-empty size must be set before rendering the bar code.");
            }
        }

        protected internal abstract void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position);
    }
}
