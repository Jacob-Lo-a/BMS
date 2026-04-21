using BMS.Core.DTOs;
using BMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) 
        { 
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {

            var order = await _orderService.CreateOrderAsync(request);
            HttpContext.Items["OrderNumber"] = order.OrderNumber;
            HttpContext.Items["UserId"] = order.UserId;

            return Ok(new
            {
                message = "訂單建立成功"
            });
        }

        [HttpGet("export-daily-report")]
        public async Task<IActionResult> Export()
        {
            await _orderService.ExportOrdersAsync();

            return Ok( new { message = "報表上傳成功"});
        }
    }
}
