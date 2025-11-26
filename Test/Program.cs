using System;
using System.Collections.Generic;
using System.Text;
using Infoware.PDF;
using Infoware.PDF.EmbeddedFonts;
using Infoware.PDF.Helpers;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace Test
{
    class Program
    {
        static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FontResolverExtensions.SetEmbeddedFontResolver();
            
            PdfDocument document = new();
            using (var generador = Generator.Instance(document))
            {
                generador
                    .NewPage();
                    //.DrawImage(@"C:\Users\vicos\Pictures\Camera Roll\WIN_20210206_16_36_37_Pro.jpg", 100, 100, new PdfSharp.Drawing.XSize(300, 300), true)
                    //.DrawImage(@"C:\Users\vicos\Pictures\Camera Roll\WIN_20210206_16_36_37_Pro.jpg", 100, 400, new PdfSharp.Drawing.XSize(300, 300), false);

                generador
                    //.DrawImage(logoStream, 30, 25, new XSize(260, 135), false)
                    .WriteBarCode39("12341213", 10, 10, new PdfSharpCore.Drawing.XSize()
                    {
                        Height = 100,
                        Width = 200,
                    })
                    //.WriteBarCode128A("01", 10, 150, new XSize(50, 40))
                    //.WriteBarCode128B("01", 10, 190, new XSize(50, 40))
                    .WriteBarCode128C("11111", 10, 230, new XSize(300, 40))
                    .WithTable(30, 480, [40, 40, 40, 40, 140], defaultRowHeight: 25)
                        .AddRow()
                            .AddCell(" Cod   1.\nPr")
                            .AddCell("Cod.\nAuxiliar")
                            .AddCell("Cantidad")
                            .AddCell("DescripciónXXXX asdasdasd asasdd asdd as")
                            .AddCell("Descripción2");

                generador
                .AddRowAutoHeight()
                    .AddCell("1")
                    .AddCell("1")
                    .AddCell("1")
                    .AddCell("Elaboración   de\n Planificación de Actividades Operativas y sdasd asd ada asd asd asdbla bla\nbla otro otro\notro otro\notro otro otro otro\notro otro otro otro ultimo")
                    .AddCell("")
                .DrawRowAutoHeight()
                .AddRowAutoHeight()
                    .AddCell("1")
                    .AddCell("1")
                    .AddCell("1")
                    .AddCell("Elaboración   de\n Planificación de Actividades Operativas y sdasd asd ada asd asd asdbla bla\nbla otro otro\notro otro\notro otro otro otro\notro otro otro otro ultimo")
                    .AddCell("")
                   .DrawRowAutoHeight();

            }
            document.Save(@"c:\test\example.pdf");
            Console.WriteLine("Hello World!");
        }
    }
}
