using BMS.Core.DTOs;
using BMS.Core.Interfaces;
using BMS.Core.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BMS.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ISftpService _sftpService;
        public OrderService(IOrderRepository orderRepository, IBookRepository bookRepository, ISftpService sftpService) 
        { 
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _sftpService = sftpService;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
         
            decimal total = 0;
            int userId = 0;

            foreach (var item in request.Items) 
            { 
                var createBookDetails = item.CreateBookDetails;
                int bookId = 0, quantity = 0;
                foreach (var detail in createBookDetails)
                {
                    bookId = detail.BookId;
                    quantity = detail.Quantity;
                }

                var stock = await _bookRepository.GetStockByIdAsync(bookId);
                var book = await _bookRepository.GetBookByIdAsync(bookId);

                if (stock == null)
                    throw new Exception("書籍不存在");

                if (stock.Quantity < quantity)
                    throw new Exception("庫存不足");

                userId = item.UserId;

                stock.Quantity -= quantity;

                var subTotal = book.BasePrice * quantity;
                total += subTotal;
            }

            var order = new Order
            {
                OrderNumber = Guid.NewGuid().ToString(),
                UserId = userId,
                TotalAmount = total,
                CreatedAt = DateTime.UtcNow,
            };
            await _orderRepository.AddOrderAsync(order);
            return order;
        }
        public async Task ExportOrdersAsync()
        {
            var today = DateTime.Today;
            var yesterdayStart = today.AddDays(-1); // 昨天 00:00:00
            var yesterdayEnd = today;               // 今天 00:00:00


            var orders = await _orderRepository.GetAllWithUserAsync(yesterdayStart, yesterdayEnd);

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("訂單報表");

            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("訂單編號");
            headerRow.CreateCell(1).SetCellValue("總金額");
            headerRow.CreateCell(2).SetCellValue("購買人帳號");
            headerRow.CreateCell(3).SetCellValue("建立時間");

            int rowIndex = 1;

            foreach (var order in orders)
            {
                IRow row = sheet.CreateRow(rowIndex);

                row.CreateCell(0).SetCellValue(order.OrderNumber);
                row.CreateCell(1).SetCellValue((double)order.TotalAmount);
                row.CreateCell(2).SetCellValue(order.User?.Username ?? "");
                row.CreateCell(3).SetCellValue(
                    order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                );
                rowIndex++;
            }

            for (int i = 0; i < 4; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using var ms = new MemoryStream();
            workbook.Write(ms);

            var fileBytes = ms.ToArray();
            var fileName = $"DailySales_{DateTime.Now:yyyyMMdd}.xlsx";

            await _sftpService.UploadReportAsync(fileBytes, fileName);
        }
    }
}
