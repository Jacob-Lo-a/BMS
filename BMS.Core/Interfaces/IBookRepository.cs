using BMS.API.DTOs;
using BMS.Core.Models;

namespace BMS.Core.Interfaces
{
    public interface IBookRepository
    {
        Task<List<BooksDto>> GetBooksWithStock();
        Task<Stock?> GetStockByIdAsync(int BookId);
        Task<Book?> GetBookByIdAsync(int id);
    }
}
