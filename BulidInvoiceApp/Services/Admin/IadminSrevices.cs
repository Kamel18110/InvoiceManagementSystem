using BulidInvoiceApp.Models;

namespace BulidInvoiceApp.Services.Admin
{
    public interface IadminSrevices
    {
        Task<CompanyInfo?> GetCompanyInfoAsync();
        Task SaveCompanyInfoAsync(CompanyInfo companyInfo);
        Task UpdateCompanyInfoAsync(CompanyInfo companyInfo);
    }
}
