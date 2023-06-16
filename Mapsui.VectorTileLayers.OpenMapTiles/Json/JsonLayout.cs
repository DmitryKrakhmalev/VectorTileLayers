using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mapsui.VectorTileLayers.OpenMapTiles.Converter;
using Mapsui.VectorTileLayers.OpenMapTiles.Expressions;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Json
{
    /// <summary>
    /// Class holding Layout data in Json format
    /// </summary>
    public class JsonLayout
    {
        [JsonProperty("line-cap")]
        public string LineCap { get; set; } = "butt";

        [JsonProperty("line-join")]
        public string LineJoin { get; set; } = "miter";

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("line-miter-limit")]
        public AdvancedFloat LineMiterLimit { get; set; } = new AdvancedFloat { SingleVal = 2f };

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("line-round-limit")]
        public AdvancedFloat LineRoundLimit { get; set; } = new AdvancedFloat { SingleVal = 1.05f };

        [JsonProperty("visibility")]
        public string Visibility { get; set; } = "visible";

        [JsonProperty("text-font")]
        public JArray TextFont { get; set; }

        [JsonConverter(typeof(AdvancedStringConverter))]
        [JsonProperty("text-field")]
        public AdvancedString TextField { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-max-width")]
        public AdvancedFloat TextMaxWidth { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-size")]
        public AdvancedFloat TextSize { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-padding")]
        public AdvancedFloat TextPadding { get; set; }

        [JsonProperty("text-offset")]
        public float[] TextOffset { get; set; }

        [JsonProperty("text-optional")]
        public bool TextOptional { get; set; }

        [JsonProperty("text-allow-overlap")]
        public bool TextAllowOverlap { get; set; }

        [JsonProperty("text-anchor")]
        public string TextAnchor { get; set; }

        [JsonProperty("text-justify")]
        public string TextJustify { get; set; }

        [JsonProperty("text-rotation-alignment")]
        public string TextRotationAlignment { get; set; }

        [JsonProperty("icon-rotation-alignment")]
        public string IconRotationAlignment { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-max-angle")]
        public AdvancedFloat TextMaxAngle { get; set; }

        [JsonProperty("text-transform")]
        public string TextTransform { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-letter-spacing")]
        public AdvancedFloat TextLetterSpacing { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-line-height")]
        public AdvancedFloat TextLineHeight { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-halo-blur")]
        public AdvancedFloat TextHaloBlur { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("text-halo-color")]
        public AdvancedColor TextHaloColor { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-halo-width")]
        public AdvancedFloat TextHaloWidth { get; set; }

        [JsonProperty("text-ignore-placement")]
        public bool TextIgnorePlacement { get; set; }

        [JsonProperty("text-keep-upright")]
        public bool TextKeepUpright { get; set; }

        [JsonProperty("text-pitch-alignment")]
        public string TextPitchAlignment { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-radial-offset")]
        public AdvancedFloat TextRadialOffset { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("text-rotate")]
        public AdvancedFloat TextRotate { get; set; }

        [JsonProperty("text-translate")]
        public float[] TextTranslate { get; set; }

        [JsonProperty("text-translate-anchor")]
        public string TextTranslateAnchor { get; set; }

        [JsonProperty("text-variable-anchor")]
        public string[] TextVariableAnchor { get; set; }

        [JsonProperty("text-writing-mode")]
        public string[] TextWritingMode { get; set; }

        [JsonConverter(typeof(AdvancedStringConverter))]
        [JsonProperty("icon-image")]
        public AdvancedString IconImage { get; set; }

        [JsonConverter(typeof(AdvancedStringConverter))]
        [JsonProperty("symbol-placement")]
        public AdvancedString SymbolPlacement { get; set; } = new AdvancedString { SingleVal = "point" };

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("symbol-spacing")]
        public AdvancedFloat SymbolSpacing { get; set; } = new AdvancedFloat { SingleVal = 250f };

        [JsonProperty("symbol-z-order")]
        public string SymbolZOrder { get; set; }

        [JsonProperty("icon-anchor")]
        public string IconAnchor { get; set; } = "center";

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("icon-color")]
        public AdvancedColor IconColor { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-halo-blur")]
        public AdvancedFloat IconHaloBlur { get; set; }

        [JsonConverter(typeof(AdvancedColorConverter))]
        [JsonProperty("icon-halo-color")]
        public AdvancedColor IconHaloColor { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-halo-width")]
        public AdvancedFloat IconHaloWidth { get; set; }

        [JsonProperty("icon-ignore-placement")]
        public bool IconIgnorePlacement { get; set; } = false;

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-padding")]
        public AdvancedFloat IconPadding { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-size")]
        public AdvancedFloat IconSize { get; set; }

        [JsonProperty("icon-offset")]
        public float[] IconOffset { get; set; }

        [JsonProperty("icon-optional")]
        public bool IconOptional { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-opacity")]
        public AdvancedFloat IconOpacity { get; set; }

        [JsonProperty("icon-allow-overlap")]
        public bool IconAllowOverlap { get; set; } = false;

        [JsonProperty("icon-keep-upright")]
        public bool IconKeepUpright { get; set; } = false;

        [JsonProperty("icon-pitch-alignment")]
        public string IconPitchAlignment { get; set; }

        [JsonConverter(typeof(AdvancedFloatConverter))]
        [JsonProperty("icon-rotate")]
        public AdvancedFloat IconRotate { get; set; }

        [JsonProperty("icon-text-fit")]
        public string IconTextFit { get; set; }

        [JsonProperty("icon-text-fit-padding")]
        public float[] IconTextFitPadding { get; set; }

        [JsonProperty("icon-translate")]
        public float[] IconTranslate { get; set; }

        [JsonProperty("icon-translate-anchor")]
        public string IconTranslateAnchor { get; set; }

        [JsonProperty("symbol-avoid-edges")]
        public bool SymbolAvoidEdges { get; set; }

        [JsonProperty("symbol-sork-key")]
        public float SymbolSortKey { get; set; }

    }
}
