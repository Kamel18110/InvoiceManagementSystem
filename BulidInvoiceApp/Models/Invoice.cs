namespace BulidInvoiceApp.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
    
        public DateTime Date { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public string NameCustomer { get; set; }
        public string? AddressCustomer { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; } =0;

       
        public List<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();
    }
}