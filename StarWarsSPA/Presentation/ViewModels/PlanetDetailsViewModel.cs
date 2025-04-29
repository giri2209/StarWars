using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;
namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for displaying details of a planet, including related films and residents.
    /// </summary>
    public class PlanetDetailsViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// The details of the selected planet.
        /// </summary>
        public Planet? Planet { get; private set; }

        /// <summary>
        /// List of films associated with the planet.
        /// </summary>
        public List<Film> Films { get; private set; } = new();

        /// <summary>
        /// List of residents of the planet.
        /// </summary>
        public List<Person> Residents { get; private set; } = new();

        /// <summary>
        /// Indicates whether the data is currently loading.
        /// </summary>
        public bool Loading { get; private set; }

        /// <summary>
        /// Contains an error message in case there is an issue loading the Planet data.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanetDetailsViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch the planet data.</param>
        public PlanetDetailsViewModel(ISwapiService swapiService) => _swapiService = swapiService;

        /// <summary>
        /// Asynchronously loads the planet details, films, and residents based on the planet ID.
        /// </summary>
        /// <param name="id">The ID of the planet to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync(string id)
        {
            try
            {
                Loading = true;

                ErrorMessage = null;
                Planet = null;

                var planet = await _swapiService.GetAsync<Planet>($"planets/{id}");

                if (planet == null)
                {
                    ErrorMessage = "Planet not found.";
                    return;
                }

                Planet = planet;

                // If the planet's films list is not null, fetch the associated films
                var filmsList = Planet?.Films ?? new List<string>(); // Use an empty list if null
                var residentsList = Planet?.Residents ?? new List<string>(); // Use an empty list if null

                // Fetch related data concurrently for better performance
                var filmsTask = _swapiService.GetManyAsync<Film>(filmsList);
                var residentsTask = _swapiService.GetManyAsync<Person>(residentsList);

                // Wait for both tasks to complete
                await Task.WhenAll(filmsTask, residentsTask);

                // Assign the fetched data to properties
                Films = await filmsTask;
                Residents = await residentsTask;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load Planet details or does not exist.";
                Console.WriteLine($"Error loading Planet details: {ex.Message}");
            }
            finally
            {
                // Ensure that loading state is set to false when the data fetch is complete
                Loading = false;
            }
        }
    }
}
