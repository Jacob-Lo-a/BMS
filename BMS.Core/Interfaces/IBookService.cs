using BMS.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Interfaces
{
    public interface IBookService
    {
        Task<List<BooksDto>> GetBooksWithStock();
    }
}
