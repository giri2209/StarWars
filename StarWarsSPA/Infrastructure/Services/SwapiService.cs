using System.Net.Http.Json;
using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Infrastructure.Services
{
    // SwapiService is the implementation of ISwapiService that interacts with the SWAPI (Star Wars API).
    public class SwapiService : ISwapiService
    {
        private readonly HttpClient _http;  // The HttpClient instance used for making requests
        private const string BaseUrl = "https://swapi.info/api/";  // Base URL for the SWAPI

        // Constructor that injects HttpClient for making HTTP requests
        public SwapiService(HttpClient http)
        {
            _http = http;
        }

        // Generic method to fetch data from the API and return the result as an object of type T
        public async Task<T?> GetAsync<T>(string relativeUrl)
        {
            try
            {
                // Sends a GET request to the API and deserializes the response into type T
                return await _http.GetFromJsonAsync<T>(BaseUrl + relativeUrl.TrimStart('/'));
            }
            catch (Exception ex)
            {
                // Logs any error that occurs during the API call
                Console.Error.WriteLine($"GetAsync<{typeof(T).Name}> error: {ex.Message}");
                return default;  // Returns the default value for the type T in case of an error
            }
        }

        // Method to fetch a list of items of type T from the API
        public async Task<List<T>> GetListAsync<T>(string endpoint)
        {
            try
            {
                // Sends a GET request to the API and deserializes the response into a List of type T
                return await _http.GetFromJsonAsync<List<T>>(BaseUrl + endpoint.TrimStart('/')) ?? new();
            }
            catch (Exception ex)
            {
                // Logs any error that occurs during the API call
                Console.Error.WriteLine($"GetListAsync<{typeof(T).Name}> error: {ex.Message}");
                return new List<T>();  // Returns an empty list in case of an error
            }
        }

        // Method to fetch multiple items from different URLs concurrently
        public async Task<List<T>> GetManyAsync<T>(IEnumerable<string> urls)
        {
            // Creates a list of tasks that will fetch each URL concurrently
            var tasks = urls.Select(async url =>
            {
                try
                {
                    // Sends a GET request for each URL and deserializes the response into type T
                    return await _http.GetFromJsonAsync<T>(url);
                }
                catch (Exception ex)
                {
                    // Logs any error that occurs while fetching the URL
                    Console.Error.WriteLine($"GetManyAsync<{typeof(T).Name}> error fetching {url}: {ex.Message}");
                    return default;  // Returns default value for the type T in case of an error
                }
            });

            // Waits for all tasks to complete and collects the results
            var results = await Task.WhenAll(tasks);
            // Filters out any null results and returns the valid ones
            return results.Where(r => r != null).ToList()!;
        }
    }
}


