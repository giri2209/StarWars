﻿using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for displaying detailed information about a specific person (character) 
    /// from the SWAPI service, including their films, starships, and species.
    /// </summary>
    public class PeopleDetailViewModel
    {
        private readonly ISwapiService _swapiService;

        /// <summary>
        /// Gets or sets the person (character) details.
        /// </summary>
        public Person? Person { get; private set; }

        /// <summary>
        /// Gets or sets the list of films in which the person appears.
        /// </summary>
        public List<Film>? Films { get; private set; }

        /// <summary>
        /// Gets or sets the list of starships the person is associated with.
        /// </summary>
        public List<Starship>? Starships { get; private set; }

        /// <summary>
        /// Gets or sets the list of species the person is associated with.
        /// </summary>
        public List<Specie>? Species { get; private set; }

        /// <summary>
        /// Indicates whether the data is currently loading.
        /// </summary>
        public bool Loading { get; private set; } = true;

        /// <summary>
        /// Contains an error message in case there is an issue loading the Character data.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleDetailViewModel"/> class.
        /// </summary>
        /// <param name="swapiService">The service used to fetch data related to the person.</param>
        public PeopleDetailViewModel(ISwapiService swapiService)
        {
            _swapiService = swapiService;
        }

        /// <summary>
        /// Asynchronously initializes the view model by loading data for the specified person.
        /// This includes retrieving films, starships, and species related to the person.
        /// </summary>
        /// <param name="id">The identifier for the person (character).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeAsync(string id)
        {
            try
            {
                ErrorMessage = null;
                Person = null;

                var person = await _swapiService.GetAsync<Person>($"people/{id}");

                if (person == null)
                {
                    ErrorMessage = "Character details not found.";
                    return;
                }

                Person = person;

                if (Person != null)
                {
                    // Ensure the lists are not null before passing them to GetManyAsync
                    var filmsList = Person.Films ?? new List<string>();  // Use an empty list if null
                    var starshipsList = Person.Starships ?? new List<string>();  // Use an empty list if null
                    var speciesList = Person.Species ?? new List<string>();  // Use an empty list if null

                    // Fetch related data concurrently for performance improvement
                    var filmsTask = _swapiService.GetManyAsync<Film>(filmsList);
                    var starshipsTask = _swapiService.GetManyAsync<Starship>(starshipsList);
                    var speciesTask = _swapiService.GetManyAsync<Specie>(speciesList);

                    // Wait for all the tasks to complete
                    await Task.WhenAll(filmsTask, starshipsTask, speciesTask);

                    // Assign the results to properties
                    Films = await filmsTask;
                    Starships = await starshipsTask;
                    Species = await speciesTask;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the data fetch
                ErrorMessage = "Failed to load Character details or does not exist.";
                Console.WriteLine($"Error loading Character details: {ex.Message}");
            }
            finally
            {
                // Ensure that loading state is set to false when data fetch is complete
                Loading = false;
            }
        }
    }
}



