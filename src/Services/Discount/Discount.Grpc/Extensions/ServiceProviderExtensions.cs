using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void MigrateDatabase<TContext>(this IServiceProvider serviceProvider, int? retry = 0)
        {
            using var scope = serviceProvider.CreateScope();

            var retryForAvailability = retry ?? 0;
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating postgresql database.");

                var connection = new NpgsqlConnection(configuration.GetConnectionString("Default"));
                connection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };

                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(
		            ID SERIAL PRIMARY KEY         NOT NULL,
		            ProductName     VARCHAR(24) NOT NULL,
		            Description     TEXT,
		            Amount          INT)";
                command.ExecuteNonQuery();

                command.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount) 
                    VALUES ('IPhone X', 'IPhone Discount', 150),
                        ('Samsung 10', 'Samsung Discount', 100)";
                command.ExecuteNonQuery();

                connection.Close();
                logger.LogInformation("Migrated postgresql database.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError(ex, "An error occured while migrating postgresql database.");

                if (retryForAvailability < 5)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase<TContext>(serviceProvider, retryForAvailability);
                }
            }
        }
    }
}
