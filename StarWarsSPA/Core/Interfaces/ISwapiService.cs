namespace StarWarsSPA.Core.Interfaces
{
    // ISwapiService defines the contract for a service that interacts with the SWAPI (Star Wars API).
    public interface ISwapiService
    {
        // Asynchronous method that fetches a single item of type T from the API using the relative URL.
        Task<T?> GetAsync<T>(string relativeUrl);

        // Asynchronous method that fetches a list of items of type T from the API using the provided endpoint.
        Task<List<T>> GetListAsync<T>(string endpoint);

        // Asynchronous method that fetches multiple items of type T from the API using a list of URLs.
        Task<List<T>> GetManyAsync<T>(IEnumerable<string> urls);
    }
}


