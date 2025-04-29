using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Presentation.ViewModels
{

    /// <summary>
    /// ViewModel for managing the details of a starship, including pilots and films it appears in.
    /// </summary>
    public class StarShipDetailsViewModel
    {
        private readonly ISwapiService swapiService;

        /// <summary>
        /// The starship details.
        /// </summary>
        public Starship? Starship { get; set; }

        /// <summary>
        /// List of pilots associated with the starship.
        /// </summary>
        public List<Pilot> Pilots { get; set; } = new();

        /// <summary>
        /// List of films in which the starship appears.
        /// </summary>
        public List<Film> Films { get; set; } = new();

        /// <summary>
        /// Indicates whether data is currently being loaded.
        /// </summary>
        public bool Loading { get; set; } = true;

        /// <summary>
        /// Contains an error message in case there is an issue loading the Starship data.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecieDetailsViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch species data.</param>
        public StarShipDetailsViewModel(ISwapiService SwapiService)
        {
            swapiService = SwapiService;
        }

        /// <summary>
        /// Initializes the starship details by fetching its data, pilots, and films.
        /// </summary>
        /// <param name="swapiService">The service used to fetch starship data.</param>
        /// <param name="id">The ID of the starship to fetch details for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync(string id)
        {
            Loading = true;

            try
            {
                // Fetch the starship details
                Starship = await swapiService.GetAsync<Starship>($"starships/{id}");

                // If the Starship's pilots list is not null, fetch the associated films
                var pilotsList = Starship?.Pilots ?? new List<string>(); // Use an empty list if null
                var filmsList = Starship?.Films ?? new List<string>(); // Use an empty list if null

                // Fetch related data concurrently for better performance
                var pilotsTask = swapiService.GetManyAsync<Pilot>(pilotsList);
                var filmsTask = swapiService.GetManyAsync<Film>(filmsList);

                // Wait for both tasks to complete
                await Task.WhenAll(pilotsTask, filmsTask);

                // Assign the fetched data to properties
                Pilots = await pilotsTask;
                Films = await filmsTask;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load Starship details or does not exist.";
                Console.WriteLine($"Error loading Starship details: {ex.Message}");
            }
            finally
            {
                Loading = false; // Ensure loading state is set to false once data is fetched
            }
        }
    }
}
