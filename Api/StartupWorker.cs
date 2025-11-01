
using Api.Data;
using Api.Models;
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

            try
            {
                //await SeedTicketData(context, cancellationToken);
            } catch (Exception ex)
            {
                var m = ex;
            }

            string envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
            if (!File.Exists(envPath))
            {
                // Handle error
            }

            foreach (var line in File.ReadAllLines(envPath))
            {
                if (string.IsNullOrEmpty(line)) continue;
                var parts = line.Split('=', 2);

                var key = parts[0].Trim();
                var value = parts[1].Trim();

                Environment.SetEnvironmentVariable(key, value);
            }
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        #endregion

        #region Methods

        private async Task SeedTicketData(ApplicationDbContext context , CancellationToken cancellationToken)
        {
            //TODO: Seed Data.
        }

        #endregion
    }
}