using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.BarCodes;
using PdfSharpCore.Pdf.Content.Objects;
using SixLabors.ImageSharp.Memory;
using static System.Net.Mime.MediaTypeNames;

namespace Infoware.PDF.Barcodes
{
    /// <summary>A Class to be able to render a Code 128 bar code</summary>
    /// <remarks>For a more detailed explanation of the Code 128, please visit the following
    /// web site: http://www.barcodeman.com/info/c128.php3
    /// or
    /// http://www.adams1.com/128code.html
    /// </remarks>
    public class Code128 : BarCode2
    {
        /// <summary>A static place holder for the patterns to draw the code 128 barcode</summary>
        public Dictionary<byte, byte[]> Patterns;
        private byte SwitchToCodC;
        private byte SwitchToCodA;
        private byte SwitchToCodB;
        private const byte CODEFNC1 = 102;
        private const byte CODESTARTA = 103;
        private const byte CODESTARTB = 104;
        private const byte CODESTARTC = 105;
        private const byte CODE128_STOPCODE = 106;

        private readonly Code_128_Code_Types Code128Code = Code_128_Code_Types.CodeB;
        private Code_128_Code_Types CurrentCode128Code;
        private List<byte> Codes;

        /// <summary>Constructor</summary>
        /// <param name="text">String - The text to be coded</param>
        /// <param name="size">XSize - The size of the bar code</param>
        public Code128(string text, XSize size)
        : this(text, size, CodeDirection.LeftToRight, Code_128_Code_Types.CodeB)
        {
        }

        /// <summary>Constructor</summary>
        /// <param name="text">String - The text to be coded</param>
        /// <param name="size">XSize - The size of the bar code</param>
        /// <param name="direction">CodeDirection - Indicates the direction to draw the bar code</param>
        public Code128(string text, XSize size, CodeDirection direction)
        : this(text, size, direction, Code_128_Code_Types.CodeB)
        {
        }

        /// <summary>Constructor</summary>
        /// <param name="text">String - The text to be coded</param>
        /// <param name="size">XSize - The size of the bar code</param>
        /// <param name="direction">CodeDirection - Indicates the direction to draw the bar code</param>
        /// <param name="code128Code">Code_128_Code_Types - Indicates which of the codes to use when rendering the bar code.
        /// The options are A, B, or buffer.</param>
        public Code128(string text, XSize size, CodeDirection direction, Code_128_Code_Types code128Code)
        : base(text, size, direction)
        {
            Text = text;
            if (Patterns == null) Load();
            Code128Code = code128Code;
            CurrentCode128Code = code128Code;

            Codes = new List<byte>()
            {
                (byte)CurrentCode128Code
            };

            switch (Code128Code)
            {
                case Code_128_Code_Types.CodeA:
                    SwitchToCodC = 99;
                    SwitchToCodB = 100;
                    break;
                case Code_128_Code_Types.CodeB:
                    SwitchToCodC = 99;
                    SwitchToCodA = 101;
                    break;
                case Code_128_Code_Types.CodeC:
                    SwitchToCodB = 100;
                    SwitchToCodA = 101;
                    break;
            }

            switch (Code128Code)
            {
                case Code_128_Code_Types.CodeA:
                case Code_128_Code_Types.CodeB:
                    text.ToCharArray().ToList().ForEach(character =>
                    {
                        Codes.Add(GetCodeABValue(character));
                    });
                    break;

                case Code_128_Code_Types.CodeC:
                    var idx = 0;
                    while (idx < text.Length)
                    {
                        if (idx + 2 >= text.Length)
                        {
                            Codes.Add(SwitchToCodB);
                            CurrentCode128Code = Code_128_Code_Types.CodeB;
                            Codes.Add(GetCodeABValue(text[idx]));
                        }
                        else
                        {
                            Codes.Add(GetCodeCValue(text.Substring(idx, 2)));
                        }
                        idx += 2;
                    }
                    break;
            }

            byte parityValue = CalculateParity();
            Codes.Add(parityValue);
            Codes.Add(CODE128_STOPCODE);
        }

        private byte CalculateParity()
        {
            long parityValue = 0;
            var i = 0;
            foreach (var code in Codes)
            {
                parityValue += (code * (i == 0 ? 1 : i));
                i++;
            }
            parityValue %= 103;
            return (byte)parityValue;
        }

        /// <summary>Creates a new instance of the Patterns field and populates it with the appropriate
        /// pattern to draw a code 128 bar code</summary>
        private void Load()
        {
            Patterns = new Dictionary<byte, byte[]>
            {
                { 0, new byte[] { 2, 1, 2, 2, 2, 2 } },
                { 1, new byte[] { 2, 2, 2, 1, 2, 2 } },
                { 2, new byte[] { 2, 2, 2, 2, 2, 1 } },
                { 3, new byte[] { 1, 2, 1, 2, 2, 3 } },
                { 4, new byte[] { 1, 2, 1, 3, 2, 2 } },
                { 5, new byte[] { 1, 3, 1, 2, 2, 2 } },
                { 6, new byte[] { 1, 2, 2, 2, 1, 3 } },
                { 7, new byte[] { 1, 2, 2, 3, 1, 2 } },
                { 8, new byte[] { 1, 3, 2, 2, 1, 2 } },
                { 9, new byte[] { 2, 2, 1, 2, 1, 3 } },
                { 10, new byte[] { 2, 2, 1, 3, 1, 2 } },
                { 11, new byte[] { 2, 3, 1, 2, 1, 2 } },
                { 12, new byte[] { 1, 1, 2, 2, 3, 2 } },
                { 13, new byte[] { 1, 2, 2, 1, 3, 2 } },
                { 14, new byte[] { 1, 2, 2, 2, 3, 1 } },
                { 15, new byte[] { 1, 1, 3, 2, 2, 2 } },
                { 16, new byte[] { 1, 2, 3, 1, 2, 2 } },
                { 17, new byte[] { 1, 2, 3, 2, 2, 1 } },
                { 18, new byte[] { 2, 2, 3, 2, 1, 1 } },
                { 19, new byte[] { 2, 2, 1, 1, 3, 2 } },
                { 20, new byte[] { 2, 2, 1, 2, 3, 1 } },
                { 21, new byte[] { 2, 1, 3, 2, 1, 2 } },
                { 22, new byte[] { 2, 2, 3, 1, 1, 2 } },
                { 23, new byte[] { 3, 1, 2, 1, 3, 1 } },
                { 24, new byte[] { 3, 1, 1, 2, 2, 2 } },
                { 25, new byte[] { 3, 2, 1, 1, 2, 2 } },
                { 26, new byte[] { 3, 2, 1, 2, 2, 1 } },
                { 27, new byte[] { 3, 1, 2, 2, 1, 2 } },
                { 28, new byte[] { 3, 2, 2, 1, 1, 2 } },
                { 29, new byte[] { 3, 2, 2, 2, 1, 1 } },
                { 30, new byte[] { 2, 1, 2, 1, 2, 3 } },
                { 31, new byte[] { 2, 1, 2, 3, 2, 1 } },
                { 32, new byte[] { 2, 3, 2, 1, 2, 1 } },
                { 33, new byte[] { 1, 1, 1, 3, 2, 3 } },
                { 34, new byte[] { 1, 3, 1, 1, 2, 3 } },
                { 35, new byte[] { 1, 3, 1, 3, 2, 1 } },
                { 36, new byte[] { 1, 1, 2, 3, 1, 3 } },
                { 37, new byte[] { 1, 3, 2, 1, 1, 3 } },
                { 38, new byte[] { 1, 3, 2, 3, 1, 1 } },
                { 39, new byte[] { 2, 1, 1, 3, 1, 3 } },
                { 40, new byte[] { 2, 3, 1, 1, 1, 3 } },
                { 41, new byte[] { 2, 3, 1, 3, 1, 1 } },
                { 42, new byte[] { 1, 1, 2, 1, 3, 3 } },
                { 43, new byte[] { 1, 1, 2, 3, 3, 1 } },
                { 44, new byte[] { 1, 3, 2, 1, 3, 1 } },
                { 45, new byte[] { 1, 1, 3, 1, 2, 3 } },
                { 46, new byte[] { 1, 1, 3, 3, 2, 1 } },
                { 47, new byte[] { 1, 3, 3, 1, 2, 1 } },
                { 48, new byte[] { 3, 1, 3, 1, 2, 1 } },
                { 49, new byte[] { 2, 1, 1, 3, 3, 1 } },
                { 50, new byte[] { 2, 3, 1, 1, 3, 1 } },
                { 51, new byte[] { 2, 1, 3, 1, 1, 3 } },
                { 52, new byte[] { 2, 1, 3, 3, 1, 1 } },
                { 53, new byte[] { 2, 1, 3, 1, 3, 1 } },
                { 54, new byte[] { 3, 1, 1, 1, 2, 3 } },
                { 55, new byte[] { 3, 1, 1, 3, 2, 1 } },
                { 56, new byte[] { 3, 3, 1, 1, 2, 1 } },
                { 57, new byte[] { 3, 1, 2, 1, 1, 3 } },
                { 58, new byte[] { 3, 1, 2, 3, 1, 1 } },
                { 59, new byte[] { 3, 3, 2, 1, 1, 1 } },
                { 60, new byte[] { 3, 1, 4, 1, 1, 1 } },
                { 61, new byte[] { 2, 2, 1, 4, 1, 1 } },
                { 62, new byte[] { 4, 3, 1, 1, 1, 1 } },
                { 63, new byte[] { 1, 1, 1, 2, 2, 4 } },
                { 64, new byte[] { 1, 1, 1, 4, 2, 2 } },
                { 65, new byte[] { 1, 2, 1, 1, 2, 4 } },
                { 66, new byte[] { 1, 2, 1, 4, 2, 1 } },
                { 67, new byte[] { 1, 4, 1, 1, 2, 2 } },
                { 68, new byte[] { 1, 4, 1, 2, 2, 1 } },
                { 69, new byte[] { 1, 1, 2, 2, 1, 4 } },
                { 70, new byte[] { 1, 1, 2, 4, 1, 2 } },
                { 71, new byte[] { 1, 2, 2, 1, 1, 4 } },
                { 72, new byte[] { 1, 2, 2, 4, 1, 1 } },
                { 73, new byte[] { 1, 4, 2, 1, 1, 2 } },
                { 74, new byte[] { 1, 4, 2, 2, 1, 1 } },
                { 75, new byte[] { 2, 4, 1, 2, 1, 1 } },
                { 76, new byte[] { 2, 2, 1, 1, 1, 4 } },
                { 77, new byte[] { 4, 1, 3, 1, 1, 1 } },
                { 78, new byte[] { 2, 4, 1, 1, 1, 2 } },
                { 79, new byte[] { 1, 3, 4, 1, 1, 1 } },
                { 80, new byte[] { 1, 1, 1, 2, 4, 2 } },
                { 81, new byte[] { 1, 2, 1, 1, 4, 2 } },
                { 82, new byte[] { 1, 2, 1, 2, 4, 1 } },
                { 83, new byte[] { 1, 1, 4, 2, 1, 2 } },
                { 84, new byte[] { 1, 2, 4, 1, 1, 2 } },
                { 85, new byte[] { 1, 2, 4, 2, 1, 1 } },
                { 86, new byte[] { 4, 1, 1, 2, 1, 2 } },
                { 87, new byte[] { 4, 2, 1, 1, 1, 2 } },
                { 88, new byte[] { 4, 2, 1, 2, 1, 1 } },
                { 89, new byte[] { 2, 1, 2, 1, 4, 1 } },
                { 90, new byte[] { 2, 1, 4, 1, 2, 1 } },
                { 91, new byte[] { 4, 1, 2, 1, 2, 1 } },
                { 92, new byte[] { 1, 1, 1, 1, 4, 3 } },
                { 93, new byte[] { 1, 1, 1, 3, 4, 1 } },
                { 94, new byte[] { 1, 3, 1, 1, 4, 1 } },
                { 95, new byte[] { 1, 1, 4, 1, 1, 3 } },
                { 96, new byte[] { 1, 1, 4, 3, 1, 1 } },
                { 97, new byte[] { 4, 1, 1, 1, 1, 3 } },
                { 98, new byte[] { 4, 1, 1, 3, 1, 1 } },
                { 99, new byte[] { 1, 1, 3, 1, 4, 1 } },
                { 100, new byte[] { 1, 1, 4, 1, 3, 1 } },
                { 101, new byte[] { 3, 1, 1, 1, 4, 1 } },
                { CODEFNC1, new byte[] { 4, 1, 1, 1, 3, 1 } },
                { CODESTARTA, new byte[] { 2, 1, 1, 4, 1, 2 } },
                { CODESTARTB, new byte[] { 2, 1, 1, 2, 1, 4 } },
                { CODESTARTC, new byte[] { 2, 1, 1, 2, 3, 2 } },
                { CODE128_STOPCODE, new byte[] { 2, 3, 3, 1, 1, 1, 2 } }
            };
        }

        /// <summary>Validates the text string to be coded</summary>
        /// <param name="text">String - The text string to be coded</param>
        protected override void CheckCode(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("Parameter text (string) can not be null");
        }

        /// <summary>Renders the content found in Text</summary>
        /// <param name="gfx">XGraphics - Instance of the drawing surface </param>
        /// <param name="brush">XBrush - Line and Color to draw the bar code</param>
        /// <param name="font">XFont - Font to use to draw the text string</param>
        /// <param name="position">XPoint - Location to render the bar code</param>
        protected internal override void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position)
        {
            XGraphicsState state = gfx.Save();

            BarCodeRenderInfo2 info = new(gfx, brush, font, position);
            InitRendering(info);
            info.CurrPosInString = 0;
            info.CurrPos = position - CalcDistance(AnchorType.TopLeft, Anchor, Size);

            Codes.ForEach(code => RenderPattern(info, GetPattern(code)));
            if (TextLocation != TextLocation.None) RenderText(info);

            gfx.Restore(state);
        }

        private void RenderPattern(BarCodeRenderInfo2 info, byte[] pattern)
        {
            XBrush space = XBrushes.White;
            for (int idx = 0; idx < pattern.Length; idx++)
            {
                if ((idx % 2) == 0)
                {
                    RenderBar(info, info.ThinBarWidth * pattern[idx]);
                }
                else
                {
                    RenderBar(info, info.ThinBarWidth * pattern[idx], space);
                }
            }
        }

        private void RenderText(BarCodeRenderInfo2 info)
        {
            if (info.Font == null) info.Font = new XFont("Courier New", Size.Height / 6);
            XPoint center = info.Position + CalcDistance(Anchor, AnchorType.TopLeft, Size);
            if (TextLocation == TextLocation.Above)
            {
                info.Gfx.DrawString(Text, info.Font, info.Brush, new XRect(center, Size), new XStringFormat()
                {
                    Alignment = XStringAlignment.Center,
                    LineAlignment = XLineAlignment.Center
                });
            }
            else if (TextLocation == TextLocation.AboveEmbedded)
            {
                XSize textSize = info.Gfx.MeasureString(Text, info.Font);
                textSize.Width += this.Size.Width * .15;
                XPoint point = info.Position;
                point.X += (this.Size.Width - textSize.Width) / 2;
                XRect rect = new(point, textSize);
                info.Gfx.DrawRectangle(XBrushes.White, rect);
                info.Gfx.DrawString(Text, info.Font, info.Brush, new XRect(center, Size), new XStringFormat()
                {
                    Alignment = XStringAlignment.Center,
                    LineAlignment = XLineAlignment.Center
                });
            }
            else if (TextLocation == TextLocation.Below)
            {
                info.Gfx.DrawString(Text, info.Font, info.Brush, new XRect(center, Size), new XStringFormat()
                {
                    Alignment = XStringAlignment.Center,
                    LineAlignment = XLineAlignment.Center
                });
            }
            else if (TextLocation == TextLocation.BelowEmbedded)
            {
                XSize textSize = info.Gfx.MeasureString(Text, info.Font);
                textSize.Width += this.Size.Width * .15;
                XPoint point = info.Position;
                point.X += (Size.Width - textSize.Width) / 2;
                point.Y += Size.Height - Size.Height;
                XRect rect = new(point, textSize);
                info.Gfx.DrawRectangle(XBrushes.White, rect);
                info.Gfx.DrawString(Text, info.Font, info.Brush, new XRect(center, Size), new XStringFormat()
                {
                    Alignment = XStringAlignment.Center,
                    LineAlignment = XLineAlignment.Center
                });
            }
        }

        private byte GetCodeCValue(string characters)
        {
            try
            {
                if (!byte.TryParse(characters, out byte result))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return result >= 0 && result <= 106 ? result : throw new ArgumentOutOfRangeException();
            }
            catch (Exception)
            {
                throw new Exception($"Error coding {characters} characters");
            }
        }

        private byte GetCodeABValue(char character)
        {
            try
            {
                var result = CurrentCode128Code switch
                {
                    Code_128_Code_Types.CodeA
                        when character < 32 => (byte)(character + 64),
                    Code_128_Code_Types.CodeA
                        when (character >= 32) && (character < 64) => (byte)(character - 32),
                    Code_128_Code_Types.CodeA => (byte)character,
                    Code_128_Code_Types.CodeB => (byte)(character - 32),
                    _ => throw new NotImplementedException()
                };
                return result >= 0 && result <= 106 ? result : throw new ArgumentOutOfRangeException();
            }
            catch (Exception)
            {
                throw new Exception($"Error coding {character} character");
            }
        }

        private byte[] GetPattern(byte codeValue)
        {
            return Patterns[codeValue];
        }

        /// <summary>Renders a single line of the character. Each character has three lines and three spaces</summary>
        /// <param name="info"></param>
        /// <param name="barWidth">Indicates the thickness of the line/bar to be rendered.</param>
        internal void RenderBar(BarCodeRenderInfo2 info, double barWidth)
        {
            RenderBar(info, barWidth, info.Brush);
        }

        /// <summary>Renders a single line of the character. Each character has three lines and three spaces</summary>
        /// <param name="info"></param>
        /// <param name="barWidth">Indicates the thickness of the line/bar to be rendered.</param>
        /// <param name="brush">Indicates the brush to use to render the line/bar.</param>
        private void RenderBar(BarCodeRenderInfo2 info, double barWidth, XBrush brush)
        {
            double height = Size.Height;
            double yPos = info.CurrPos.Y;

            switch (TextLocation)
            {
                case TextLocation.Above:
                    yPos = info.CurrPos.Y + (height / 5);
                    height *= 4.0 / 5;
                    break;
                case TextLocation.Below:
                    height *= 4.0 / 5;
                    break;
                case TextLocation.AboveEmbedded:
                case TextLocation.BelowEmbedded:
                case TextLocation.None:
                    break;
            }
            XRect rect = new XRect(info.CurrPos.X, yPos, barWidth, height);
            info.Gfx.DrawRectangle(brush, rect);
            info.CurrPos.X += barWidth;
        }

        internal override void InitRendering(BarCodeRenderInfo2 info)
        {
            if (Codes == null) throw new InvalidOperationException("A text must be set before rendering the bar code.");
            if (Codes.Count == 0) throw new InvalidOperationException("A non-empty size must be set before rendering the bar code.");

            int numberOfBars = Codes.Count; // The length of the string plus the start, stop, and parity value
            numberOfBars *= 11; // Each character has 11 bars
            numberOfBars += 2; // Add two more because the stop bit has two extra bars

            // Calculating the width of a bar
            info.ThinBarWidth = ((double)this.Size.Width / (double)numberOfBars);
        }
    }


    /// <summary>Code types for Code 128 bar code</summary>
    public enum Code_128_Code_Types : byte
    {
        /// <summary>Code A</summary>
        CodeA = 103,
        /// <summary>Code B</summary>
        CodeB = 104,
        /// <summary>Code buffer</summary>
        CodeC = 105,
    }
}