using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string username)
        {
            return await _dataContext
                .Orders
                .Where(o => o.UserName == username)
                .ToListAsync();
        }
    }
}
