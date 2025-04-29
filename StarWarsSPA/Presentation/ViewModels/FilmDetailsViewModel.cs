using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;
namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing the details of a specific film.
    /// It interacts with the <see cref="ISwapiService"/> to fetch data related to the film,
    /// such as characters, starships, planets, and species, and manages the loading state
    /// and error messages for the film details view.
    /// </summary>
    public class FilmDetailsViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// Gets the details of the film.
        /// </summary>
        public Film? Film { get; private set; }

        /// <summary>
        /// Gets a list of characters associated with the film.
        /// </summary>
        public List<Person> Characters { get; private set; } = new();

        /// <summary>
        /// Gets a list of starships associated with the film.
        /// </summary>
        public List<Starship> Starships { get; private set; } = new();

        /// <summary>
        /// Gets a list of planets associated with the film.
        /// </summary>
        public List<Planet> Planets { get; private set; } = new();

        /// <summary>
        /// Gets a list of species associated with the film.
        /// </summary>
        public List<Specie> Species { get; private set; } = new();

        /// <summary>
        /// Indicates whether the data for the film is currently loading.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// Contains an error message in case there is an issue loading the film data.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilmDetailsViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service that provides access to the Star Wars API.</param>
        public FilmDetailsViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Loads the details of the film based on the given ID. This method retrieves
        /// the film data and its associated characters, starships, planets, and species.
        /// It also manages the loading state and handles any errors that may occur during
        /// the data retrieval process.
        /// </summary>
        /// <param name="id">The unique identifier of the film to be loaded.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InitializeAsync(string id)
        {
            try
            {
                // Start loading process
                Loading = true;
                ErrorMessage = null;
                Film = null;

                var film = await _swapiService.GetAsync<Film>($"films/{id}");

                if (film == null)
                {
                    ErrorMessage = "Film details not found.";
                    return;
                }

                Film = film;

                // If the film has characters, starships, planets, or species, fetch them asynchronously
                Characters = await _swapiService.GetManyAsync<Person>(Film.Characters ?? new List<string>());
                Starships = await _swapiService.GetManyAsync<Starship>(Film.Starships ?? new List<string>());
                Planets = await _swapiService.GetManyAsync<Planet>(Film.Planets ?? new List<string>());
                Species = await _swapiService.GetManyAsync<Specie>(Film.Species ?? new List<string>());
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the data fetch
                ErrorMessage = "Failed to load Film details or does not exist.";
                Console.WriteLine($"Error loading film details: {ex.Message}");
            }
            finally
            {
                // Ensure that the loading state is set to false regardless of success or failure
                Loading = false;
            }
        }
    }
}

