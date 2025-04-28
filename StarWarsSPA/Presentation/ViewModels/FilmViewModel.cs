using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for managing and displaying a list of films with pagination and search capabilities.
    /// </summary>
    public class FilmViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// Gets the list of all films retrieved from the API.
        /// </summary>
        public List<Film> Films { get; private set; } = new();

        /// <summary>
        /// Gets the filtered list of films based on the search query.
        /// </summary>
        public List<Film> FilteredFilms { get; private set; } = new();

        /// <summary>
        /// Indicates whether the data is currently loading.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// Gets or sets the current page number for pagination.
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        private const int ItemsPerPage = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilmViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch films data.</param>
        public FilmViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Gets the total number of pages for the filtered list of films.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(FilteredFilms.Count / (double)ItemsPerPage);

        /// <summary>
        /// Gets the films for the current page, based on pagination.
        /// </summary>
        public List<Film> PaginatedFilms =>
            FilteredFilms.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();

        /// <summary>
        /// Initializes the view model by fetching and sorting the films.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync()
        {
            Loading = true;
            try
            {
                // Fetch films and order by EpisodeId
                Films = (await _swapiService.GetListAsync<Film>("films")).OrderBy(f => f.EpisodeId).ToList();
                FilteredFilms = Films; // Initially no filtering
            }
            catch (Exception ex)
            {
                // Handle the error gracefully, e.g., show a message to the user
                // You could also log the exception here for debugging purposes
                Console.WriteLine($"Error loading films: {ex.Message}");
            }
            finally
            {
                Loading = false;
            }
        }

        /// <summary>
        /// Filters the list of films based on the provided search query.
        /// </summary>
        /// <param name="query">The search query used to filter the films by title.</param>
        public void HandleSearch(string query)
        {
            // Filter films based on query
            FilteredFilms = Films
                .Where(f => !string.IsNullOrEmpty(f.Title) && f.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Reset to first page on search
            CurrentPage = 1;
        }
    }
}

