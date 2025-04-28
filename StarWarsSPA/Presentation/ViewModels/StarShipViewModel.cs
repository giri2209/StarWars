using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for managing the list and pagination of starships.
    /// </summary>
    public class StarShipViewModel
    {

        private readonly ISwapiService _swapiService;

        /// <summary>
        /// The list of all available starships.
        /// </summary>
        public List<Starship> Starships { get; private set; } = new();

        /// <summary>
        /// The list of FilteredStarShips starships based on the search query.
        /// </summary>
        public List<Starship> FilteredStarShips { get; private set; } = new();

        /// <summary>
        /// Indicates whether data is currently being loaded.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// The current page number for pagination.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// The number of starships to display per page.
        /// </summary>
        public int ItemsPerPage { get; set; } = 9;

        /// <summary>
        /// The paginated starships for the current page.
        /// </summary>
        public IEnumerable<Starship> PaginatedStarships =>
            FilteredStarShips.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);

        /// <summary>
        /// The total number of pages available based on the FilteredStarShips list.
        /// </summary>
        public int TotalPages =>
            (int)Math.Ceiling((double)FilteredStarShips.Count / ItemsPerPage);

        /// <summary>
        /// Initializes a new instance of the <see cref="StarShipViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch vehicle data.</param>
        public StarShipViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Initializes the view model by fetching all starships from the service.
        /// </summary>
        /// <param name="swapiService">The service used to fetch starship data.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync(ISwapiService swapiService)
        {
            Loading = true;

            try
            {
                // Fetch all starships from the service
                Starships = await swapiService.GetListAsync<Starship>("starships") ?? new List<Starship>();
                FilteredStarShips = Starships;
            }
            catch (Exception ex)
            {
                // Log any errors during the data fetch process
                Console.Error.WriteLine($"Failed to fetch starships: {ex.Message}");
            }
            finally
            {
                Loading = false;
            }
        }

        /// <summary>
        /// Filters the starship list based on the search query.
        /// </summary>
        /// <param name="query">The search query used to filter the starships by name.</param>
        public void HandleSearch(string query)
        {
            FilteredStarShips = Starships
                .Where(s => !string.IsNullOrEmpty(s.Name) && s.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
            CurrentPage = 1; // Reset to the first page after a new search
        }

        /// <summary>
        /// Extracts the ID from a given URL.
        /// </summary>
        /// <param name="url">The URL string from which the ID will be extracted.</param>
        /// <returns>The ID extracted from the URL.</returns>
        public string GetIdFromUrl(string url) =>
            url.TrimEnd('/').Split('/').Last();
    }
}
