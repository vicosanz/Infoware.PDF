using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoware.PDF;

public class Rectangle(GeneratorPage generatorPage, double x, double y, double width)
{
    public GeneratorPage GeneratorPage { get; init; } = generatorPage;
    public double X { get; init; } = x;
    public double Y { get; init; } = y;
    public double Width { get; init; } = width;
}
