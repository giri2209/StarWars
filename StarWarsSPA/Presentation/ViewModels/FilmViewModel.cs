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
        /// Stores any error message encountered during data loading.
        /// </summary>
        public string? ErrorMessage { get; private set; }

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
        /// Indicates if the current page is the first page.
        /// </summary>
        public bool IsFirstPage => CurrentPage == 1;

        /// <summary>
        /// Indicates if the current page is the last page.
        /// </summary>
        public bool IsLastPage => CurrentPage == TotalPages;


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
                ErrorMessage = "Failed to load films. Please try again later.";
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
            FilteredFilms = string.IsNullOrWhiteSpace(query) ? Films
            : Films.Where(f => !string.IsNullOrEmpty(f.Title) && f.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
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

