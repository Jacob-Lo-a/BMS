using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.DTOs
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItem> Items { get; set; } = new();
    }
}
