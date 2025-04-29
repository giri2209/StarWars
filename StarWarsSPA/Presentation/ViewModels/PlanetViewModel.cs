using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;
namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for managing a list of planets, including filtering, pagination, and data loading.
    /// </summary>
    public class PlanetViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// List of all planets.
        /// </summary>
        public List<Planet> Planets { get; private set; } = new();

        /// <summary>
        /// List of planets after applying filters (e.g., search query).
        /// </summary>
        public List<Planet> FilteredPlanets { get; private set; } = new();

        /// <summary>
        /// Indicates whether data is currently loading.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// The current page of the paginated planets list.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// The number of items to display per page.
        /// </summary>
        public int ItemsPerPage { get; set; } = 9;

        /// <summary>
        /// Total number of pages based on the filtered list and items per page.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)FilteredPlanets.Count / ItemsPerPage);

        /// <summary>
        /// Indicates if the current page is the first page.
        /// </summary>
        public bool IsFirstPage => CurrentPage == 1;

        /// <summary>
        /// Indicates if the current page is the last page.
        /// </summary>
        public bool IsLastPage => CurrentPage == TotalPages;


        /// <summary>
        /// Gets a paginated list of planets based on the current page.
        /// </summary>
        public IEnumerable<Planet> PaginatedPlanets => FilteredPlanets
            .Skip((CurrentPage - 1) * ItemsPerPage)
            .Take(ItemsPerPage);

        /// <summary>
        /// Stores any error message encountered during data loading.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanetViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch planet data.</param>
        public PlanetViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Asynchronously loads the list of planets from the service and applies initial filtering.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync()
        {
            try
            {
                Loading = true;

                // Fetch the list of planets asynchronously
                var result = await _swapiService.GetListAsync<Planet>("planets");

                // If result is null, initialize as empty list
                Planets = result ?? new List<Planet>();

                // Initially set the filtered list to be the same as the full list
                FilteredPlanets = Planets;

            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load Planets. Please try again later.";
                // Log the error (can be replaced with a more advanced logging mechanism)
                Console.WriteLine($"Error loading planets: {ex.Message}");
            }
            finally
            {
                // Ensure loading is set to false when the data fetch is complete
                Loading = false;
            }
        }

        /// <summary>
        /// Handles the search query and filters the planets list.
        /// </summary>
        /// <param name="query">The search query to filter planets by name.</param>
        public void HandleSearch(string query)
        {
            FilteredPlanets = string.IsNullOrWhiteSpace(query) ? Planets
            : Planets.Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
             .ToList();

            CurrentPage = 1;

        }

        public void GoToNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
        }

        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }
    }
}
