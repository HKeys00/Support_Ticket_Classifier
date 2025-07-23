
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Shared.Helpers;
using Shared.Models;
namespace Api
{
    /// <summary>
    /// Background task used to establish and seed data into 
    /// the database.
    /// </summary>
    public class StartupWorker : IHostedService
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialises a new instance of the <see cref="StartupWorker"/> class.
        /// </summary>
        /// <param name="serviceProvider">An instance of <see cref="IServiceProvider"/>.</param>
        public StartupWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Implementation of IHostedService

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync(cancellationToken);

            await SeedTicketData(context);
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        #endregion

        #region Methods

        private async Task SeedTicketData(ApplicationDbContext context)
        {
            using TextFieldParser parser = new TextFieldParser(Path.Combine(Directory.GetCurrentDirectory(), "Data\\customer_support_tickets.csv"));
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;

            string[]? headers = parser.ReadFields();
            
            while(!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                if (fields == null) continue;

                var ticket = CsvReader.Parse<Ticket>(fields, headers);
            }

            var m = 0;
        }

        #endregion
    }
}
