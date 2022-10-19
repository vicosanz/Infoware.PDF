using System;
using System.Collections.Generic;
using System.Text;
using Infoware.PDF;
using Infoware.PDF.Helpers;
using PdfSharpCore.Pdf;

namespace Test
{
    class Program
    {
        static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            PdfDocument document = new();
            using (var generador = Generator.Instance(document))
            {
                generador
                    .NewPage();
                    //.DrawImage(@"C:\Users\vicos\Pictures\Camera Roll\WIN_20210206_16_36_37_Pro.jpg", 100, 100, new PdfSharp.Drawing.XSize(300, 300), true)
                    //.DrawImage(@"C:\Users\vicos\Pictures\Camera Roll\WIN_20210206_16_36_37_Pro.jpg", 100, 400, new PdfSharp.Drawing.XSize(300, 300), false);

                generador
                    .WithTable(30, 450, new List<double>() { 40, 40, 40, 140, 140 }, defaultRowHeight: 25)
                        .AddRow()
                            .AddCell("Cod.\nPrincipal")
                            .AddCell("Cod.\nAuxiliar")
                            .AddCell("Cantidad")
                            .AddCell("Descripción")
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
            document.Save(@"d:\test\example.pdf");
            Console.WriteLine("Hello World!");
        }
    }
}
