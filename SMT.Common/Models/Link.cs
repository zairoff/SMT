using Newtonsoft.Json;
using System.ComponentModel;

namespace SMT.ViewModel.Models
{
    public class Link
    {
        public const string GetMethod = "Get";

        public static Link To(string routeName, object routeValues = null) => new()
        {
            RouteName = routeName,
            RouteValues = routeValues,
            Method = GetMethod
        };

        [JsonProperty(Order = -4)]
        public string Href { get; set; }

        [JsonProperty(Order = -3,
                    PropertyName = "rel",
                    NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }

        [JsonProperty(Order = -2,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }

        [JsonIgnore]
        public string RouteName { get; set; }

        [JsonIgnore]
        public object RouteValues { get; set; }
    }
}
