using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.DTOs
{
    public class CreateOrderItem
    {
        public int UserId { get; set; }
        public List<CreateBookDetail> CreateBookDetails { get; set; } = new();
    }

    public class CreateBookDetail
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
