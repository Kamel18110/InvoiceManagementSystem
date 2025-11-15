using System.ComponentModel.DataAnnotations;

namespace BulidInvoiceApp.Models
{
    public class InvoiceDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid InvoiceID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
    }
}