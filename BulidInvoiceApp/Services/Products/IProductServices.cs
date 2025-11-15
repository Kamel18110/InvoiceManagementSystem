using BulidInvoiceApp.Models;

namespace BulidInvoiceApp.Services.Products
{
    public interface IProductServices
    {
         Task<int> Create(Product product);
        Task<int> Update(Product product);
        Task Delete(Guid id);
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);
    }
}
