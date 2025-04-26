using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;
namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for handling species details, including fetching associated films and people.
    /// </summary>
    public class SpecieDetailsViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// Gets or sets the species information.
        /// </summary>
        public Specie? Species { get; private set; }

        /// <summary>
        /// List of films that the species appeared in.
        /// </summary>
        public List<Film> Films { get; private set; } = new List<Film>();

        /// <summary>
        /// List of people belonging to the species.
        /// </summary>
        public List<Person> People { get; private set; } = new List<Person>();

        /// <summary>
        /// Indicates whether the data is still loading.
        /// </summary>
        public bool Loading { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecieDetailsViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch species data.</param>
        public SpecieDetailsViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Initializes the species details by fetching the species data and its related films and people.
        /// </summary>
        /// <param name="speciesId">The species ID to fetch details for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Initialize(string speciesId)
        {
            Loading = true;
            try
            {
                // Fetch the species data asynchronously
                Species = await _swapiService.GetAsync<Specie>($"species/{speciesId}");

                // If species has films, fetch all related films asynchronously
                if (Species?.Films != null)
                    Films = await FetchAll<Film>(Species.Films);

                // If species has people, fetch all related people asynchronously
                if (Species?.People != null)
                    People = await FetchAll<Person>(Species.People);
            }
            catch (Exception ex)
            {
                // Log the error (can be replaced with a more advanced logging mechanism)
                Console.Error.WriteLine($"Failed to fetch species details: {ex.Message}");
            }
            finally
            {
                // Set loading to false once data fetching is complete
                Loading = false;
            }
        }

        /// <summary>
        /// Helper method to fetch a list of items asynchronously from a collection of URLs.
        /// </summary>
        /// <typeparam name="T">The type of items to fetch.</typeparam>
        /// <param name="urls">The URLs to fetch data from.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of fetched items.</returns>
        private async Task<List<T>> FetchAll<T>(IEnumerable<string> urls)
        {
            // Create a collection of tasks to fetch data asynchronously for each URL
            var tasks = urls.Select(url => _swapiService.GetAsync<T>(url));

            try
            {
                // Wait for all tasks to complete
                var results = await Task.WhenAll(tasks);

                // Filter out any null results and return a list of non-nullable items
                return results.Where(result => result != null).ToList()!;
            }
            catch (Exception ex)
            {
                // Log any error that occurs while fetching the data
                Console.Error.WriteLine($"Failed to fetch items: {ex.Message}");
                return new List<T>(); // Return an empty list in case of an error
            }
        }
    }
}
