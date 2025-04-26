using StarWarsSPA.Core.Interfaces;
using StarWarsSPA.Core.Models;

namespace StarWarsSPA.Presentation.ViewModels
{
    /// <summary>
    /// ViewModel for managing the details of a starship, including pilots and films it appears in.
    /// </summary>
    public class StarShipDetailsViewModel
    {
        /// <summary>
        /// The starship details.
        /// </summary>
        public Starship? Starship { get; set; }

        /// <summary>
        /// List of pilots associated with the starship.
        /// </summary>
        public List<SimpleItem> Pilots { get; set; } = new();

        /// <summary>
        /// List of films in which the starship appears.
        /// </summary>
        public List<SimpleItem> Films { get; set; } = new();

        /// <summary>
        /// Indicates whether data is currently being loaded.
        /// </summary>
        public bool Loading { get; set; } = true;

        /// <summary>
        /// Initializes the starship details by fetching its data, pilots, and films.
        /// </summary>
        /// <param name="swapiService">The service used to fetch starship data.</param>
        /// <param name="id">The ID of the starship to fetch details for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Initialize(ISwapiService swapiService, string id)
        {
            Loading = true;

            try
            {
                // Fetch the starship details
                Starship = await swapiService.GetAsync<Starship>($"starships/{id}");

                // Fetch pilots if available
                if (Starship?.Pilots != null)
                    Pilots = await swapiService.GetManyAsync<SimpleItem>(Starship.Pilots);

                // Fetch films if available
                if (Starship?.Films != null)
                    Films = await swapiService.GetManyAsync<SimpleItem>(Starship.Films);
            }
            catch (Exception ex)
            {
                // Log any errors during the data fetch process
                Console.Error.WriteLine($"Failed to fetch starship details: {ex.Message}");
            }
            finally
            {
                Loading = false; // Ensure loading state is set to false once data is fetched
            }
        }
    }
}
