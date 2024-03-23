using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AssetsNet.API.Models.Stock
{
    public class HeaderStockData
    {
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public string? Exchange { get; set; }
        [JsonPropertyName("mic_code")]
        public string? MicCode { get; set; }
        public string? Currency { get; set; }
        public string? Timestamp { get; set; }
        public string? Datetime { get; set; }
        public string? Open { get; set; }
        public string? Low { get; set; }
        public string? High { get; set; }
        public string? Close { get; set; }
        public string? Volume { get; set; }
        [JsonProperty("previous_close")]
        public string? PreviousClose { get; set; }
        public string? Change { get; set; }
        [JsonProperty("percent_change")]
        public string? PercentChange  { get; set; }
        [JsonProperty("average_volume")]
        public string? AverageVolume { get; set; }
        [JsonProperty("is_market_open")]
        public bool? IsMarketOpen { get; set; }
    }
}
