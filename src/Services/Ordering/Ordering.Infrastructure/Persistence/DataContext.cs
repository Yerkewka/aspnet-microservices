using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) 
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public override int SaveChanges()
        {
            OnSaveChanges();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnSaveChanges();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnSaveChanges();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnSaveChanges();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnSaveChanges()
        {
            foreach (var entityEntry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entityEntry.State)
                {
                    case EntityState.Modified:
                        entityEntry.Entity.LastModifiedDate = DateTime.UtcNow;
                        entityEntry.Entity.LastModifiedBy = "yerkewka";
                        break;
                    case EntityState.Added:
                        entityEntry.Entity.CreatedDate = DateTime.UtcNow;
                        entityEntry.Entity.CreatedBy = "yerkewka";
                        break;
                    case EntityState.Deleted:
                        //if (entityEntry.CurrentValues.Properties.Any(p => p.Name == "CreatedDate")
                        //    && entityEntry.CurrentValues.Properties.Any(p => p.Name == "IsDeleted"))
                        //{
                        //    entityEntry.CurrentValues["IsDeleted"] = true;
                        //    entityEntry.State = EntityState.Modified;
                        //}
                        break;
                    default:
                        throw new ArgumentNullException();
                }
            }
        }
    }
}
