using System;

namespace ClientCode.Products.ReadModels
{
    public class ProductListItem
    {
        public int Id { get; set; }
        public string Pn { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Line { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
