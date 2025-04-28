using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Presentation.ViewModels
{
    public class VehicleViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// The list of all vehicles fetched from the service.
        /// </summary>
        private List<Vehicle> Vehicles = new();

        /// <summary>
        /// The list of filtered vehicles based on the search query.
        /// </summary>
        private List<Vehicle> FilteredVehicles = new();

        /// <summary>
        /// Indicates whether data is currently being loaded.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// The current page of the paginated vehicles.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        private const int ItemsPerPage = 9;

        /// <summary>
        /// The total number of pages based on the filtered list of vehicles.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)FilteredVehicles.Count / ItemsPerPage);

        /// <summary>
        /// Indicates if the current page is the first page.
        /// </summary>
        public bool IsFirstPage => CurrentPage == 1;

        /// <summary>
        /// Indicates if the current page is the last page.
        /// </summary>
        public bool IsLastPage => CurrentPage == TotalPages;

        /// <summary>
        /// A collection of vehicles for the current page, based on pagination.
        /// </summary>
        public IEnumerable<Vehicle> PaginatedVehicles => FilteredVehicles
            .Skip((CurrentPage - 1) * ItemsPerPage)
            .Take(ItemsPerPage);

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch vehicle data.</param>
        public VehicleViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Initializes the vehicle view model by fetching the list of vehicles.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync()
        {
            Loading = true;

            try
            {
                // Fetch all vehicles from the service
                Vehicles = await _swapiService.GetListAsync<Vehicle>("vehicles") ?? new List<Vehicle>();
                FilteredVehicles = Vehicles;
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the fetching process
                Console.Error.WriteLine($"Failed to load vehicles: {ex.Message}");
                // Optionally, you could add a user-friendly error message or log it to a system
            }
            finally
            {
                Loading = false;
            }
        }

        /// <summary>
        /// Handles the search functionality by filtering the vehicles based on the provided query.
        /// </summary>
        /// <param name="query">The search query entered by the user.</param>
        public void HandleSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // Reset to the original list of vehicles if the query is empty or whitespace
                FilteredVehicles = Vehicles;
            }
            else
            {
                // Filter the vehicles based on the query (case-insensitive)
                FilteredVehicles = Vehicles
                    .Where(v => !string.IsNullOrEmpty(v.Name) && v.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Always reset to the first page after a search
            CurrentPage = 1;
        }

        /// <summary>
        /// Extracts the ID from a URL.
        /// </summary>
        /// <param name="url">The URL from which to extract the ID.</param>
        /// <returns>The extracted ID from the URL.</returns>
        public string GetIdFromUrl(string url) =>
            url.TrimEnd('/').Split('/').Last();
    }
}
