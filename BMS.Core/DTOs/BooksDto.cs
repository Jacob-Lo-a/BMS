namespace BMS.API.DTOs
{
    public class BooksDto
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Author { get; set; }
        public decimal BasePrice { get; set; }
    }
}
