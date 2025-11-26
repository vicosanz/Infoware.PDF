namespace Infoware.PDF.Helpers;

public static class GeneratorHelper
{
    /// <summary>
    /// Set Margins for new Pages
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <param name="top">Top Margin</param>
    /// <param name="bottom">Bottom Margin</param>
    /// <returns>The generator</returns>
    public static IGenerator Margins(this IGenerator generator, double top, double bottom)
    {
        generator.MarginTop = top;
        generator.MarginBottom = bottom;
        return generator;
    }

    /// <summary>
    /// Get a copy of the current Page and Y Position
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <param name="pagePointer">GeneratorPage</param>
    /// <returns>The generator</returns>
    public static IGenerator GetPagePointer(this IGenerator generator, out GeneratorPage pagePointer)
    {
        pagePointer = generator.PagePointer.Clone();
        return generator;
    }

    /// <summary>
    /// Restore cursor to an specific Page and Y position
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <param name="pagePointer">GeneratorPage</param>
    /// <returns>The generator</returns>
    public static IGenerator SetPagePointer(this IGenerator generator, GeneratorPage pagePointer)
    {
        generator.PagePointer = pagePointer;
        return generator;
    }

    /// <summary>
    /// Set Y position to current Page
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <param name="Y">Y position</param>
    /// <returns>The generator</returns>
    public static IGenerator SetPointerY(this IGenerator generator, double Y)
    {
        generator.PagePointer.PointY = Y;
        return generator;
    }

    /// <summary>
    /// Create a new page and set as current
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <returns>The generator</returns>
    public static IGenerator NewPage(this IGenerator generator)
    {
        generator.AddPage();
        return generator;
    }

    /// <summary>
    /// Turn on or off the output of the following commands depending of the value of expression until EndIf
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <param name="expression">Boolean expression</param>
    /// <returns>The generator</returns>
    public static IGenerator If(this IGenerator generator, bool expression)
    {
        generator.Expression = expression;
        return generator;
    }

    /// <summary>
    /// Turn on outputs
    /// </summary>
    /// <param name="generator">The generator</param>
    /// <returns>The generator</returns>
    public static IGenerator Endif(this IGenerator generator)
    {
        generator.Expression = true;
        return generator;
    }

}
