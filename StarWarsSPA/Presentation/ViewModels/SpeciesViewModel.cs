using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;
namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for handling the list of species, including search and pagination.
    /// </summary>
    public class SpeciesViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// List of all species.
        /// </summary>
        public List<Specie> Species { get; private set; } = new List<Specie>();

        /// <summary>
        /// List of species after filtering by search query.
        /// </summary>
        public List<Specie> FilteredSpecies { get; private set; } = new List<Specie>();

        /// <summary>
        /// The current page number for pagination.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// The number of items displayed per page.
        /// </summary>
        public int ItemsPerPage { get; set; } = 9;

        /// <summary>
        /// Indicates whether data is still being loaded.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// Gets the total number of pages based on the filtered species.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)FilteredSpecies.Count / ItemsPerPage);

        /// <summary>
        /// Gets the species for the current page based on pagination.
        /// </summary>
        public IEnumerable<Specie> PaginatedSpecies => FilteredSpecies.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage);

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch species data.</param>
        public SpeciesViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Initializes the species list by fetching data from the API and handling pagination.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync()
        {
            Loading = true;
            var allSpecies = new List<Specie>();

            try
            {
                    // Fetch a list of species from the API
                    var response = await _swapiService.GetListAsync<Specie>("species");
                    if (response.Any())
                    {
                        allSpecies = response;
                    }
        

                // Store the fetched species and set the filtered list to be the same initially
                Species = allSpecies;
                FilteredSpecies = allSpecies;
            }
            catch (Exception ex)
            {
                // Log any errors encountered during data fetching
                Console.Error.WriteLine($"Failed to load species: {ex.Message}");
            }
            finally
            {
                Loading = false;
            }
        }

        /// <summary>
        /// Filters the species based on the search query.
        /// </summary>
        /// <param name="query">The search query string.</param>
        public void HandleSearch(string query)
        {
            FilteredSpecies = Species
                .Where(s => !string.IsNullOrEmpty(s.Name) && s.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
            CurrentPage = 1; // Reset to the first page after search
        }
    }
}
