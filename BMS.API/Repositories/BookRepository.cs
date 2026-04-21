using BMS.API.DTOs;
using BMS.Core.Interfaces;
using BMS.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BMS.API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly CloudBookStoreDbContext _context;
        public BookRepository(CloudBookStoreDbContext context) 
        { 
            _context = context;
        }

        public async Task<List<BooksDto>> GetBooksWithStock()
        {
            return await _context.Books
                .AsNoTracking()
                .Select(b => new BooksDto 
                { 
                    ISBN = b.Isbn,
                    Title = b.Title,
                    Category = b.Category.Name,
                    Author = b.Author.Name,
                    BasePrice = b.BasePrice
                })
                .ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int BookId)
        {
            return await _context.Stocks.FirstOrDefaultAsync(b =>  b.BookId == BookId);
        }
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
