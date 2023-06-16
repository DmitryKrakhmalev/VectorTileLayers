using Newtonsoft.Json;
using System.Collections.Generic;
using Mapsui.VectorTileLayers.OpenMapTiles.Converter;
using Mapsui.VectorTileLayers.OpenMapTiles.Expressions;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Json
{
    public class JsonPaint
    {
        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("background-color")]
        public AdvancedColor BackgroundColor { get; set; }

        [JsonConverter(typeof(AdvancedStringConverter))]
        [JsonProperty("background-pattern")]
        public AdvancedString BackgroundPattern { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("background-opacity")]
        public AdvancedFloat BackgroundOpacity { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("fill-color")]
        public AdvancedColor FillColor { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("fill-opacity")]
        public AdvancedFloat FillOpacity { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("line-color")]
        public AdvancedColor LineColor { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("line-width")]
        public AdvancedFloat LineWidth { get; set; }

        [JsonProperty("fill-translate")]
        public object FillTranslate { get; set; }

        [JsonConverter(typeof(AdvancedStringConverter))]
        [JsonProperty("fill-pattern")]
        public AdvancedString FillPattern { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("fill-outline-color")]
        public AdvancedColor FillOutlineColor { get; set; }

        [JsonConverter(typeof(StoppedFloatArrayConverter))]
        [JsonProperty("line-dasharray")]
        public StoppedFloatArray LineDashArray { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("line-opacity")]
        public AdvancedFloat LineOpacity { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("text-color")]
        public AdvancedColor TextColor { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-halo-width")]
        public AdvancedFloat TextHaloWidth { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("text-halo-color")]
        public AdvancedColor TextHaloColor { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-halo-blur")]
        public AdvancedFloat TextHaloBlur { get; set; }

        [JsonConverter(typeof(StoppedBooleanConverter))]
        [JsonProperty("fill-antialias")]
        public StoppedBoolean FillAntialias { get; set; }

        [JsonProperty("fill-translate-anchor")]
        public string FillTranslateAnchor { get; set; }

        [JsonProperty("line-gap-width")]
        public object LineGapWidth { get; set; }

        [JsonProperty("line-blur")]
        public object LineBlur { get; set; }

        [JsonProperty("line-translate")]
        public IList<int> LineTranslate { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-halo-blur")]
        public AdvancedFloat IconHaloBlur { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("icon-halo-color")]
        public AdvancedColor IconHaloColor { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-halo-width")]
        public AdvancedFloat IconHaloWidth { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-opacity")]
        public AdvancedFloat TextOpacity { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("icon-color")]
        public AdvancedColor IconColor { get; set; }

        [JsonProperty("text-translate")]
        public IList<int> TextTranslate { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-opacity")]
        public AdvancedFloat IconOpacity { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("raster-opacity")]
        public AdvancedFloat RasterOpacity { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("raster-hue-rotate")]
        public AdvancedFloat RasterHueRotate { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("raster-brightness-min")]
        public AdvancedFloat RasterBrightnessMin { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("raster-brightness-max")]
        public AdvancedFloat RasterBrightnessMax { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("raster-saturation")]
        public AdvancedFloat RasterSaturation { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("raster-contrast")]
        public AdvancedFloat RasterContrast { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("raster-fade-duration")]
        public AdvancedFloat RasterFadeDuration { get; set; }
    }
}
