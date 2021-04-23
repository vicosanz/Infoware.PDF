using System;
using System.Text;
using Infoware.PDF;
using Infoware.PDF.Helpers;
using PdfSharp.Pdf;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            PdfDocument document = new();
            using (var generador = Generator.Instance(document))
            {
                generador
                    .NewPage()
                    .DrawImage(@"C:\Users\vicos\Pictures\Camera Roll\WIN_20210206_16_36_37_Pro.jpg", 100, 100, new PdfSharp.Drawing.XSize(300, 300), true)
                    .DrawImage(@"C:\Users\vicos\Pictures\Camera Roll\WIN_20210206_16_36_37_Pro.jpg", 100, 400, new PdfSharp.Drawing.XSize(300, 300), false);
            }
            document.Save(@"C:\Users\vicos\Desktop\example.pdf");
            Console.WriteLine("Hello World!");
        }
    }
}
