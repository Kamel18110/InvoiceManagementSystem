using BulidInvoiceApp.Models;

namespace BulidInvoiceApp.Services.Invoices
{
    public interface IInvoiceService
    {
        Task<int> Create(Invoice invoice);
        Task<IEnumerable<Invoice>> GetAll();
        Task<Invoice> GetById(Guid id);
        Task<int> Update(Invoice invoice);
        Task<bool> Delete(Guid id);
        Task<int> CreateInvoiceDetail(InvoiceDetail detail);
        Task<bool> DeleteInvoiceDetail(Guid id);
        Task<IEnumerable<InvoiceDetail>> GetInvoiceDetails(Guid invoiceId);
    }
}