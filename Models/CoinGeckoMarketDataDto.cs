// In Models/CoinGeckoMarketDataDto.cs
using System.Text.Json.Serialization;

namespace AccessibleCryptoViewer.Models // Ensure namespace matches
{
    // This DTO matches the structure of objects in the JSON array
    // returned by CoinGecko's /coins/markets endpoint
    public class CoinGeckoMarketDataDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; } // URL to the image

        [JsonPropertyName("current_price")]
        public decimal? CurrentPrice { get; set; }

        [JsonPropertyName("market_cap")]
        public decimal? MarketCap { get; set; }

        [JsonPropertyName("total_volume")]
        public decimal? TotalVolume { get; set; }

        [JsonPropertyName("price_change_percentage_24h")]
        public double? PriceChangePercentage24h { get; set; }

        [JsonPropertyName("circulating_supply")]
        public decimal? CirculatingSupply { get; set; }

        [JsonPropertyName("total_supply")]
        public decimal? TotalSupply { get; set; }

        // Add other fields as needed from the API response
    }
}