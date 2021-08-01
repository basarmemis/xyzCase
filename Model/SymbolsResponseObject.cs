using System.Text.Json.Serialization;

namespace Case.Model
{
    public class SymbolsResponseObject
    {
        [JsonPropertyName("AAPL")]
        public Symbol AAPL { get; set; }

        [JsonPropertyName("MSFT")]
        public Symbol MSFT { get; set; }

        [JsonPropertyName("EUR/USD")]
        public Symbol EURUSD { get; set; }

        [JsonPropertyName("SBUX")]
        public Symbol SBUX { get; set; }

        [JsonPropertyName("NKE")]
        public Symbol NKE { get; set; }
    }
}