using Newtonsoft.Json;

namespace Case.Model
{
    //[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Meta
    {
        public string symbol { get; set; }
        public string interval { get; set; }
        public string currency { get; set; }
        public string exchange_timezone { get; set; }
        public string exchange { get; set; }
        public string type { get; set; }

        [JsonProperty("currency_base", NullValueHandling = NullValueHandling.Ignore)]
        public string currency_base { get; set; }
        public string currency_quote { get; set; }
    }
}