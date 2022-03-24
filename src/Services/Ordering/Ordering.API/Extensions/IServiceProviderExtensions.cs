using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static void MigrateDatabase<TContext>(this IServiceProvider serviceProvider, 
            Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
            var retryForAvailability = retry ?? 0;

            using var scope = serviceProvider.CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation("Starting database migration associated with context {ContextName}", typeof(TContext).Name);

                context.Database.Migrate();
                seeder(context, services);

                logger.LogInformation("Finished database migration associated with context {ContextName}", typeof(TContext).Name);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "Exception was thrown while migrating database");

                if (retryForAvailability < 5)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase<TContext>(serviceProvider, seeder, retryForAvailability);
                }
            }
        }
    }
}
