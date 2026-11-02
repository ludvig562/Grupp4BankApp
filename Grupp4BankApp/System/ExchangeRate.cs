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
        public decimal EurRate { get; set; }

        public decimal UsdRate { get; set; }

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
        private static decimal eurRate;
        private static decimal usdRate;

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
            return lastUpdated.HasValue &&
                   lastUpdated.Value.Date == DateTime.Today;
        }

        // Download one exchange rate and its publication date
        private static async Task<(decimal Rate, DateTime Date)> FetchRateAsync(
            string currency)
        {
            string url =
                $"https://api.frankfurter.dev/v2/rate/{currency}/SEK";

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

        // Download and display both rates before asking for approval
        public static async Task UpdateFromApiAsync()
        {
            try
            {
                Console.WriteLine("Hämtar växelkurser...\n");

                var eur = await FetchRateAsync("EUR");
                var usd = await FetchRateAsync("USD");

                Console.WriteLine(
                    $"1 EUR = {eur.Rate} SEK " +
                    $"(kursdatum: {eur.Date:yyyy-MM-dd})");

                Console.WriteLine(
                    $"1 USD = {usd.Rate} SEK " +
                    $"(kursdatum: {usd.Date:yyyy-MM-dd})");

                Console.WriteLine("\nVill du spara dessa kurser?");
                Console.WriteLine("[1] Ja");
                Console.WriteLine("[2] Nej");

                string choice = Console.ReadLine() ?? "";

                if (choice != "1")
                {
                    Console.WriteLine("Kurserna sparades inte.\n");
                    return;
                }

                // Create the data that will be saved to the file
                ExchangeRateData ratesToSave = new ExchangeRateData
                {
                    EurRate = eur.Rate,
                    UsdRate = usd.Rate,
                    LastUpdated = DateTime.Today
                };

                // Convert the rates to JSON and save them to a file
                string json = JsonSerializer.Serialize(ratesToSave,new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(ratesFilePath, json);

                // Update the rates in memory after the file has been saved
                eurRate = ratesToSave.EurRate;
                usdRate = ratesToSave.UsdRate;
                lastUpdated = ratesToSave.LastUpdated;

                Console.WriteLine("Växelkurserna har sparats.\n");
            }
            catch (HttpRequestException)
            {
                Console.WriteLine(
                    "Kunde inte hämta växelkurser. Kontrollera internetanslutningen.");
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Förfrågan tog för lång tid. Försök igen.");
            }
            catch (JsonException)
            {
                Console.WriteLine("Kunde inte läsa svaret från valutatjänsten.");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Valutatjänsten skickade ogiltiga uppgifter.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Valutatjänsten skickade ett ogiltigt format.");
            }
            catch (global::System.Collections.Generic.KeyNotFoundException)
            {
                Console.WriteLine("Valutatjänstens svar saknar uppgifter.");
            }
            catch (IOException)
            {
                Console.WriteLine("Kunde inte spara växelkurserna till fil.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Saknar behörighet att spara växelkurserna.");
            }
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