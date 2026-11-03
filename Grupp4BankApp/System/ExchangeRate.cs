using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Grupp4BankApp.System
{
    // Data stored in the exchange rates file
    internal class ExchangeRateData
    {
        public decimal EurRate { get; set; } = 10.9596m;

        public decimal UsdRate { get; set; } = 9.789m;

        public DateTime LastUpdated { get; set; }
    }
    internal static class ExchangeRates
    {
        // Reuse the same HTTP client for all requests
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        // Store the value of one EUR and one USD in SEK
        private static decimal eurRate = 10.9596m;
        private static decimal usdRate = 9.789m;

        // Store the date when the administrator approved the rates
        private static DateTime? lastUpdated;

        // Store the file next to the application
        private static readonly string ratesFilePath = Path.Combine(AppContext.BaseDirectory, "exchangeRates.json");

        // Load saved rates when this class is first used
        static ExchangeRates()
        {
            LoadRates();
        }

        // Read saved rates from the JSON file
        private static void LoadRates()
        {
            if (!File.Exists(ratesFilePath))
            {
                return;
            }

            try
            {
                string json = File.ReadAllText(ratesFilePath);

                ExchangeRateData? savedRates = JsonSerializer.Deserialize<ExchangeRateData>(json);

                // Only load valid rates
                if (savedRates == null ||
                    savedRates.EurRate <= 0 ||
                    savedRates.UsdRate <= 0)
                {
                    return;
                }

                eurRate = savedRates.EurRate;
                usdRate = savedRates.UsdRate;
                lastUpdated = savedRates.LastUpdated;
            }
            catch (IOException)
            {
                Console.WriteLine("Kunde inte läsa sparade växelkurser.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Saknar behörighet att läsa sparade växelkurser.");
            }
            catch (JsonException)
            {
                Console.WriteLine("Filen med växelkurser har ett ogiltigt format.");
            }
        }

        public static bool UpdatedToday()
        {
            return lastUpdated.HasValue && lastUpdated.Value.Date == DateTime.Today;
        }

        // Download one exchange rate and its publication date
        private static async Task<(decimal Rate, DateTime Date)> FetchRateAsync(string currency)
        {
            string url = $"https://api.frankfurter.dev/v2/rate/{currency}/SEK";

            string json = await client.GetStringAsync(url);

            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            decimal rate = root.GetProperty("rate").GetDecimal();
            DateTime date = root.GetProperty("date").GetDateTime();

            if (rate <= 0)
            {
                throw new InvalidOperationException("Invalid exchange rate.");
            }

            return (rate, date);
        }

        //method for downloading rates
        public static async Task<(decimal EurRate, decimal UsdRate, DateTime EurDate, DateTime UsdDate)> GetLatestRatesAsync()
        {
            var eur = await FetchRateAsync("EUR");
            var usd = await FetchRateAsync("USD");

            return (eur.Rate, usd.Rate, eur.Date, usd.Date);
        }

        //save downloaded rates
        public static void SaveRates(decimal eurRateToSave, decimal usdRateToSave)
        {
            ExchangeRateData ratesToSave = new ExchangeRateData
            {
                EurRate = eurRateToSave,
                UsdRate = usdRateToSave,
                LastUpdated = DateTime.Today
            };

            string json = JsonSerializer.Serialize(
                ratesToSave,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(ratesFilePath, json);

            // Update values in memory
            eurRate = ratesToSave.EurRate;
            usdRate = ratesToSave.UsdRate;
            lastUpdated = ratesToSave.LastUpdated;
        }

        // Return the value of one currency unit in SEK
        public static decimal GetRate(Enums.CurrencyTypes currency)
        {
            switch (currency)
            {
                case Enums.CurrencyTypes.SEK:
                    return 1m;

                case Enums.CurrencyTypes.EUR:
                    return eurRate;

                case Enums.CurrencyTypes.USD:
                    return usdRate;

                default:
                    throw new ArgumentException("Unknown currency.");
            }
        }
    }
}