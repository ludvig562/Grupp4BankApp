using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Grupp4BankApp.System
{
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

                // Replace existing rates only after both requests succeed
                // and the administrator approves the new values
                eurRate = eur.Rate;
                usdRate = usd.Rate;
                lastUpdated = DateTime.Today;

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