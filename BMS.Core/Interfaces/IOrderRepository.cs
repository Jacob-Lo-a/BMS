using BMS.Core.Models;

namespace BMS.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<List<Order>> GetAllWithUserAsync(DateTime start, DateTime end);
    }
}
