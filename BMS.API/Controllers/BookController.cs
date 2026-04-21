using BMS.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [Authorize(Roles = "Member")]
        [HttpGet("getBooks")]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _bookService.GetBooksWithStock();
            return Ok(result);
        }
    }
}
