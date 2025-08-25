using Microsoft.AspNetCore.Components;
using Shared.Models;
namespace Client.Components.Pages
{
    /// <summary>
    /// The ticket kanban board page.
    /// </summary>
    public partial class Tickets
    {
        #region Fields

        private List<Ticket> _tickets;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injected <see cref="IHttpClientFactory"/>.
        /// </summary>
        [Inject]
        public required IHttpClientFactory ClientFactory { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            _tickets = new List<Ticket>();

            var client = ClientFactory.CreateClient("Api");

            var response = await client.GetFromJsonAsync<List<Ticket>>("ticket") ;
            if (response != null)
            {
                _tickets = response;
            }
        }

        #endregion
    }
}
