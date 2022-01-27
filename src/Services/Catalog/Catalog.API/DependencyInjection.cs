using Catalog.API.Data;
using Catalog.API.Repositories;

namespace Catalog.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInternalDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
