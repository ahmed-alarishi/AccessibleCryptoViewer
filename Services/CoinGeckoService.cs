// In Services/CoinGeckoService.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers; // << ADDED
using System.Net.Http.Json; // For ReadFromJsonAsync
using System.Text.Json;    // For JsonException and general JSON processing
using System.Threading.Tasks;
using System.Linq;
using AccessibleCryptoViewer.Models; 
using AccessibleCryptoViewer; // Added for RuntimeLogger

namespace AccessibleCryptoViewer.Services 
{
    public class CoinGeckoService
    {
        private static readonly HttpClient _httpClient;

        // Static constructor to initialize HttpClient once
        static CoinGeckoService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.coingecko.com/api/v3/")
            };
            // Set a default User-Agent header
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("AccessibleCryptoViewer/1.0 (+http://yourprojecturl.example.com; for-learning-purpose)");
        }

        public CoinGeckoService() { } 

        public async Task<List<CryptoCurrency>> GetMarketDataAsync(IEnumerable<string> coinIds, string vsCurrency = "usd")
        {
            var idsString = string.Join(",", coinIds);
            // Added some common parameters that CoinGecko /coins/markets uses
            var requestUri = $"coins/markets?vs_currency={vsCurrency}&ids={idsString}&order=market_cap_desc&per_page=100&page=1&sparkline=false&price_change_percentage=24h";
            
            List<CryptoCurrency> cryptoList = new List<CryptoCurrency>();

            try
            {
                // Allow dtoList to be null if API returns null or deserialization fails to list
                List<CoinGeckoMarketDataDto>? dtoList = await _httpClient.GetFromJsonAsync<List<CoinGeckoMarketDataDto>>(requestUri);

                if (dtoList != null) // Check if dtoList is not null before iterating
                {
                    foreach (var dto in dtoList)
                    {
                        // Ensure dto itself isn't null if the list can contain nulls (though GetFromJsonAsync typically wouldn't put nulls in the list itself if it succeeds)
                        if (dto == null) continue; 

                        cryptoList.Add(new CryptoCurrency
                        {
                            Id = dto.Id ?? string.Empty, // CoinGecko ID
                            Name = dto.Name ?? "N/A",
                            Symbol = dto.Symbol?.ToUpper(), // Will be null if dto.Symbol is null; CryptoCurrency.Symbol is now string?
                            ImageUrl = dto.Image ?? string.Empty,
                            PriceUsd = dto.CurrentPrice,
                            MarketCapUsd = dto.MarketCap,
                            Volume24hUsd = dto.TotalVolume,
                            PriceChangePercentage24h = dto.PriceChangePercentage24h,
                            CirculatingSupply = dto.CirculatingSupply,
                            TotalSupply = dto.TotalSupply,
                            LastUpdated = DateTime.UtcNow, 
                            DataSourceApi = "CoinGecko"
                            // Initialize other properties of CryptoCurrency to defaults or from DTO if available
                        });
                    }
                }
                else
                {
                     RuntimeLogger.Log($"CoinGecko API returned null or empty data for market data from URI: {requestUri}");
                }
            }
            catch (HttpRequestException ex)
            {
                RuntimeLogger.Log($"CoinGecko API Request Error: {ex.StatusCode} - {ex.Message} for URI: {requestUri}");
            }
            catch (NotSupportedException ex) // Potentially thrown by GetFromJsonAsync if content type is invalid
            {
                RuntimeLogger.Log($"CoinGecko JSON Content Error (NotSupported): {ex.Message} for URI: {requestUri}");
            }
            catch (JsonException ex) // Specific exception for JSON parsing issues
            {
                RuntimeLogger.Log($"CoinGecko JSON Deserialization Error: {ex.Message} (Line: {ex.LineNumber}, Pos: {ex.BytePositionInLine}) for URI: {requestUri}");
            }
            catch (Exception ex) // Catch-all for other unexpected errors
            {
                RuntimeLogger.Log($"An unexpected error occurred while fetching CoinGecko data: {ex.Message} for URI: {requestUri}");
            }

            return cryptoList;
        }
    }
}