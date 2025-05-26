namespace OpenglTestConsole.Classes.API.JSON
{
    public class MCSDFJSON
    {
        #region font classes
        public class Atlas
        {
            public string type { get; set; }
            public float distanceRange { get; set; }
            public float distanceRangeMiddle { get; set; }
            public float size { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string yOrigin { get; set; }
        }

        public class Metrics
        {
            public float emSize { get; set; }
            public float lineHeight { get; set; }
            public float ascender { get; set; }
            public float descender { get; set; }
            public float underlineY { get; set; }
            public float underlineThickness { get; set; }
        }

        public class Bounds
        {
            public float left { get; set; }
            public float bottom { get; set; }
            public float right { get; set; }
            public float top { get; set; }
        }

        public class Glyph
        {
            public int unicode { get; set; }
            public float advance { get; set; }
            public Bounds planeBounds { get; set; } // Optional: some glyphs like space may not have this
            public Bounds atlasBounds { get; set; }
        }

        public class FontJson
        {
            public Atlas atlas { get; set; }
            public Metrics metrics { get; set; }
            public List<Glyph> glyphs { get; set; }
        }
        #endregion


        public static FontJson? GetFontJson(string jsonPath) =>
            LoadJsonFromFile<FontJson>.Load(jsonPath)!;
    }
}
