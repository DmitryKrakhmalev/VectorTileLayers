﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Json
{
    /// <summary>
    /// Class holding StoppedBoolean data in Json format
    /// </summary>
    public class JsonStoppedBoolean
    {
        [JsonProperty("stops")]
        public IList<KeyValuePair<float, bool>> Stops { get; set; }
    }
}
