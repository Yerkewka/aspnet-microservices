using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class DataContextSeed
    {
        public static async Task SeedAsync(DataContext dataContext, ILogger<DataContextSeed> logger)
        {
            if (!dataContext.Orders.Any())
            {
                dataContext.Orders.AddRange(GetPreconfiguredOrders());

                await dataContext.SaveChangesAsync();

                logger.LogInformation("Seeded the database with context {dbContextName}", typeof(DataContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new() { 
                    UserName = "swn", 
                    FirstName = "Sanjyot", 
                    LastName = "Agureddy", 
                    EmailAddress = "sanjyot.agureddy@hotmail.com", 
                    AddressLine = "Pune", 
                    Country = "India", 
                    TotalPrice = 350 
                }
            };
        }
    }
}
