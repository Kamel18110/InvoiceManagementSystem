using System.ComponentModel.DataAnnotations;

namespace BulidInvoiceApp.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public byte[]? Image { get; set; }
        public int Status { get; set; } = 0;
    }
}
