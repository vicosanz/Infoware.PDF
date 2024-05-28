using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Internal;
using SixLabors.Fonts;

namespace PdfSharpCore.Utils;

public static class FontResolverExtensions
{
    public static void SetEmbeddedFontResolver()
    {
        GlobalFontSettings.FontResolver = new EmbeddedFontResolver();
    }
}

public class EmbeddedFontResolver : IFontResolver
{
    private readonly struct FontFileInfo
    {
        public string Path { get; }

        public FontDescription FontDescription { get; }

        public string FamilyName => FontDescription.FontFamilyInvariantCulture;

        private FontFileInfo(string path, FontDescription fontDescription)
        {
            Path = path;
            FontDescription = fontDescription;
        }

        public XFontStyle GuessFontStyle()
        {
            return FontDescription.Style switch
            {
                FontStyle.Bold => XFontStyle.Bold,
                FontStyle.Italic => XFontStyle.Italic,
                FontStyle.BoldItalic => XFontStyle.BoldItalic,
                _ => XFontStyle.Regular,
            };
        }

        public static FontFileInfo Load(string path)
        {
            FontDescription fontDescription = FontDescription.LoadDescription(path);
            return new FontFileInfo(path, fontDescription);
        }
    }

    private static readonly Dictionary<string, FontFamilyModel> InstalledFonts;

    private static readonly string[] SSupportedFonts;

    public string DefaultFontName => "Arial";

    public bool NullIfFontNotFound { get; set; }

    static EmbeddedFontResolver()
    {
        InstalledFonts = [];
        SSupportedFonts = Directory.GetFiles("./Fonts/", "*.ttf", SearchOption.AllDirectories);
        SetupFontsFiles(SSupportedFonts);
    }

    public static void SetupFontsFiles(string[] sSupportedFonts)
    {
        List<FontFileInfo> list = [];
        foreach (string path in sSupportedFonts)
        {
            try
            {
                FontFileInfo item = FontFileInfo.Load(path);
                list.Add(item);
            }
            catch (Exception)
            {
            }
        }

        foreach (IGrouping<string, FontFileInfo> item2 in from info in list
                                                          group info by info.FamilyName)
        {
            try
            {
                string key = item2.Key;
                FontFamilyModel value = DeserializeFontFamily(key, item2);
                InstalledFonts.Add(key.ToLower(), value);
            }
            catch (Exception)
            {
            }
        }
    }

    private static FontFamilyModel DeserializeFontFamily(string fontFamilyName, IEnumerable<FontFileInfo> fontList)
    {
        FontFamilyModel fontFamilyModel = new FontFamilyModel
        {
            Name = fontFamilyName
        };
        if (fontList.Count() == 1)
        {
            fontFamilyModel.FontFiles.Add(XFontStyle.Regular, fontList.First().Path);
        }
        else
        {
            foreach (FontFileInfo font in fontList)
            {
                XFontStyle key = font.GuessFontStyle();
                if (!fontFamilyModel.FontFiles.ContainsKey(key))
                {
                    fontFamilyModel.FontFiles.Add(key, font.Path);
                }
            }
        }

        return fontFamilyModel;
    }

    public virtual byte[] GetFont(string faceFileName)
    {
        using MemoryStream memoryStream = new MemoryStream();
        string text = "";
        try
        {
            text = SSupportedFonts.ToList().First((string x) => x.ToLower().Contains(Path.GetFileName(faceFileName).ToLower()));
            using Stream stream = File.OpenRead(text);
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0L;
            return memoryStream.ToArray();
        }
        catch (Exception value)
        {
            Console.WriteLine(value);
            throw new Exception("No Font File Found - " + faceFileName + " - " + text);
        }
    }

    public virtual FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (InstalledFonts.Count == 0)
        {
            throw new FileNotFoundException("No Fonts installed on this device!");
        }

        if (InstalledFonts.TryGetValue(familyName.ToLower(), out var value))
        {
            string value4;
            if (isBold && isItalic)
            {
                if (value.FontFiles.TryGetValue(XFontStyle.BoldItalic, out var value2))
                {
                    return new FontResolverInfo(Path.GetFileName(value2));
                }
            }
            else if (isBold)
            {
                if (value.FontFiles.TryGetValue(XFontStyle.Bold, out var value3))
                {
                    return new FontResolverInfo(Path.GetFileName(value3));
                }
            }
            else if (isItalic && value.FontFiles.TryGetValue(XFontStyle.Italic, out value4))
            {
                return new FontResolverInfo(Path.GetFileName(value4));
            }

            if (value.FontFiles.TryGetValue(XFontStyle.Regular, out var value5))
            {
                return new FontResolverInfo(Path.GetFileName(value5));
            }

            return new FontResolverInfo(Path.GetFileName(value.FontFiles.First().Value));
        }

        if (NullIfFontNotFound)
        {
            return null;
        }

        return new FontResolverInfo(Path.GetFileName(InstalledFonts.First().Value.FontFiles.First().Value));
    }
}
