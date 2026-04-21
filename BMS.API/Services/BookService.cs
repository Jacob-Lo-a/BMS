using BMS.API.DTOs;
using BMS.Core.Interfaces;
namespace BMS.API.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<List<BooksDto>> GetBooksWithStock()
        {
            return await _bookRepository.GetBooksWithStock();
        }
    }
}
