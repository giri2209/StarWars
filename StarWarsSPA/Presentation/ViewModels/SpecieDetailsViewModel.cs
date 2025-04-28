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
        public async Task InitializeAsync(string speciesId)
        {
            Loading = true;
            try
            {
                Species = await _swapiService.GetAsync<Specie>($"species/{speciesId}");

                // If the specie's films list is not null, fetch the associated films
                var filmsList = Species?.Films ?? new List<string>(); // Use an empty list if null
                var residentsList = Species?.People ?? new List<string>(); // Use an empty list if null

                // Fetch related data concurrently for better performance
                var filmsTask = _swapiService.GetManyAsync<Film>(filmsList);
                var residentsTask = _swapiService.GetManyAsync<Person>(residentsList);

                // Wait for both tasks to complete
                await Task.WhenAll(filmsTask, residentsTask);

                // Assign the fetched data to properties
                Films = await filmsTask;
                People = await residentsTask;
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
    }
}
