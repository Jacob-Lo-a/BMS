using System;
using System.Collections.Generic;

namespace BMS.Core.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int CategoryId { get; set; }

    public int AuthorId { get; set; }

    public decimal BasePrice { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
