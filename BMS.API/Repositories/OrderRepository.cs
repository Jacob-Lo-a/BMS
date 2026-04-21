using BMS.Core.Interfaces;
using BMS.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BMS.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CloudBookStoreDbContext _context;

        public OrderRepository(CloudBookStoreDbContext context) 
        { 
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync()) 
            {
                try
                {
                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception) 
                { 
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }

        public async Task<List<Order>> GetAllWithUserAsync(DateTime start, DateTime end)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Where(o => o.CreatedAt >= start && o.CreatedAt < end)
                .ToListAsync();
        }
    }
}
