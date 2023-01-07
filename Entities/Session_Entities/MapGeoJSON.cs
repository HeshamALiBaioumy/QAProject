using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA.Entities.Session_Entities
{
    public class Properties
    {
    }

    public class Geometry
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("coordinates")]
        public List<object> coordinates { get; set; }
        //public List<List<List<point>>> coordinates { get; set; }
    }

    public class point
    {
        [JsonProperty("lat")]
        string lat { get; set; }

        [JsonProperty("lng")]
        string lng { get; set; }
    }

    public class Feature
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("properties")]
        public Properties properties { get; set; }

        [JsonProperty("geometry")]
        public Geometry geometry { get; set; }
    }

    public class MapGeoJSON
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("features")]
        public List<Feature> features { get; set; }
    }
}