using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Presentation.ViewModels
{
    public class VehicleDetailViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// The vehicle details fetched from the service.
        /// </summary>
        public Vehicle? Vehicle { get; private set; }

        /// <summary>
        /// The list of pilots associated with the vehicle.
        /// </summary>
        public List<Pilot> Pilots { get; private set; } = new();

        /// <summary>
        /// The list of films associated with the vehicle.
        /// </summary>
        public List<Film> Films { get; private set; } = new();

        /// <summary>
        /// Indicates whether data is currently being loaded.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// Contains an error message in case there is an issue loading the Vehicle data.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleDetailViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch vehicle data.</param>
        public VehicleDetailViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Initializes the view model by fetching the vehicle details along with related pilots and films.
        /// </summary>
        /// <param name="id">The identifier of the vehicle to be fetched.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync(string id)
        {
            Loading = true;

            try
            {
                // Fetch the vehicle by ID
                Vehicle = await _swapiService.GetAsync<Vehicle>($"vehicles/{id}");

                if (Vehicle != null)
                {
                    // Fetch associated pilots if available
                    Pilots = Vehicle.Pilots?.Any() == true
                        ? await _swapiService.GetManyAsync<Pilot>(Vehicle.Pilots)
                        : new List<Pilot>();

                    // Fetch associated films if available
                    Films = Vehicle.Films?.Any() == true
                        ? await _swapiService.GetManyAsync<Film>(Vehicle.Films)
                        : new List<Film>();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load Vehicle details or does not exist.";
                Console.WriteLine($"Error loading Vehicle details: {ex.Message}");
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
