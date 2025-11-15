using BulidInvoiceApp.Models;

namespace BulidInvoiceApp.Services.Pdf
{
    public interface IPdfInvoiceService
    {
        Task<byte[]> GenerateInvoicePdf(Invoice invoice, List<InvoiceProductInfo> products, CompanyInfo companyInfo);
    }

}
