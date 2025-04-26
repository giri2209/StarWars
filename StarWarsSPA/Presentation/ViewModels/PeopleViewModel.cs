using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Presentation.ViewModels
{
    public class PeopleViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// The list of all people fetched from the service.
        /// </summary>
        private List<Person> _people = new();

        /// <summary>
        /// The list of filtered people based on the search query.
        /// </summary>
        private List<Person> _filteredPeople = new();

        /// <summary>
        /// Indicates whether data is currently being loaded.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// The current page of the paginated people list.
        /// </summary>
        public int CurrentPage { get; private set; } = 1;

        private const int ItemsPerPage = 8;

        /// <summary>
        /// The total number of pages based on the filtered list of people.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)_filteredPeople.Count / ItemsPerPage);

        /// <summary>
        /// Indicates if the current page is the first page.
        /// </summary>
        public bool IsFirstPage => CurrentPage == 1;

        /// <summary>
        /// Indicates if the current page is the last page.
        /// </summary>
        public bool IsLastPage => CurrentPage == TotalPages;

        /// <summary>
        /// A collection of people for the current page, based on pagination.
        /// </summary>
        public IEnumerable<Person> PaginatedPeople => _filteredPeople
            .Skip((CurrentPage - 1) * ItemsPerPage)
            .Take(ItemsPerPage);

        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch people data.</param>
        public PeopleViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Initializes the people view model by fetching the list of people from the service.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Initialize()
        {
            Loading = true;

            try
            {
                // Fetch all people from the service
                _people = await _swapiService.GetListAsync<Person>("people") ?? new List<Person>();
                _filteredPeople = _people;
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the fetching process
                Console.Error.WriteLine($"Failed to load people: {ex.Message}");
                // Optionally, you could add a user-friendly error message or log it to a system
            }
            finally
            {
                Loading = false;
            }
        }

        /// <summary>
        /// Handles the search functionality by filtering the list of people based on the provided query.
        /// </summary>
        /// <param name="query">The search query entered by the user.</param>
        public void HandleSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // Reset to the original list of people if the query is empty or whitespace
                _filteredPeople = _people;
            }
            else
            {
                // Filter the people based on the query (case-insensitive)
                _filteredPeople = _people
                    .Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Always reset to the first page after a search
            CurrentPage = 1;
        }

        /// <summary>
        /// Moves to the previous page of paginated people, if possible.
        /// </summary>
        public void PrevPage()
        {
            if (CurrentPage > 1) CurrentPage--;
        }

        /// <summary>
        /// Moves to the next page of paginated people, if possible.
        /// </summary>
        public void NextPage()
        {
            if (CurrentPage < TotalPages) CurrentPage++;
        }
    }
}