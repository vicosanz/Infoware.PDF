# Infoware.PDF
 PDFSharpNetStandard2 Fluent Wrapper

Usage:
```csharp
        static readonly Style NormalStyle = new(new("Verdana", 7, XFontStyle.Regular), XBrushes.Black);
        static readonly Style NormalBoldStyle = new(new("Verdana", 7, XFontStyle.Bold), XBrushes.Black);
        public static IGenerator UseNormalStyle(this IGenerator generator) => generator.WithStyle(NormalStyle);
        public static IGenerator UseNormalBoldStyle(this IGenerator generator) => generator.WithStyle(NormalBoldStyle);
        ...
        
            var companyName = "Infoware Soluciones";
            
            PdfDocument document = new();
            using (var generator = Generator.Instance(document))
            {
                generator
                    .Rectangle(new XRect(30, 365, 540, 75))
                    .UseNormalBoldStyle()
                    .Write("Company:", 35, 380)
                    .UseNormalStyle()
                    .Write(companyName, 250, 380);
            ...
            
                generator
                    .WithTable(30, 450, new List<double>() { 150, 150 }, defaultRowHeight: 25)
                        .AddRow()
                            .AddCell("Developer")
                            .AddCell("Status")
                        .AddRow()
                            .AddCell("Victor Sanchez")
                            .AddCell("Ready");

                //create current page and position pointer
                generator
                    .GetPagePointer(out var pointerFinItems);
                    
                ....
                
                //back to pointer
                generator
                    .SetPagePointer(pointerFinItems)
                    .WithTable(30, generator.PointerY + 25, new List<double>() { 100, 100 }, defaultRowHeight: 15)
                        .AddRow(50)
                            .AddCell("Developer")
                            .AddCell("Status")
                        .AddRow() //use defaultRowHeight from current Table
                            .AddCell("Victor Sanchez")
                            .AddCell("Ready");
                    
                           
            document.Save(pathToPDFFile);
            //done
            
```
