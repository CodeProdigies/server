using PdfSharp.Fonts;
using PdfSharp.Quality;
using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;

namespace prod_server.Classes
{
    public class CustomFontResolver : IFontResolver
    {
        public byte[]? GetFont(string faceName)
        {
            switch (faceName)
            {
                case "Courier New":
                    return LoadEmbeddedFont("Courier New");

                default:
                    return FontHelper.LoadFontData(faceName);
            }
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo(familyName, isBold, isItalic);
        }

        private byte[] LoadEmbeddedFont(string fontName)
        {
            // Replace this with your actual code to load the font file data
            // This example assumes the font file is embedded as a resource
            var assembly = typeof(CustomFontResolver).Assembly;
            var resourceName = $"{assembly.GetName().Name}.Fonts.{fontName}.ttf";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Font file for '{fontName}' not found.");
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
