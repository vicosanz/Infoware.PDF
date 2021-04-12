using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoware.PDF
{
    public class Rectangle
    {
        public GeneratorPage GeneratorPage { get; init; }
        public double X { get; init; }
        public double Y { get; init; }
        public double Width { get; init; }

        public Rectangle(GeneratorPage generatorPage, double x, double y, double width)
        {
            GeneratorPage = generatorPage;
            X = x;
            Y = y;
            Width = width;
        }
    }
}
