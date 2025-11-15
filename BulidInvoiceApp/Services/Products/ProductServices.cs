using BulidInvoiceApp.Models;
using BulidInvoiceApp.Storages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BulidInvoiceApp.Services.Products
{
    public class ProductServices : IProductServices
    {
        private readonly DatabaseHelper _dbHelper;

        public ProductServices(DatabaseHelper dbHelper)
        {
            this._dbHelper = dbHelper;
        }
        public async Task<int> Create(Product product)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", product.Id),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Description", product.Description ?? (object)DBNull.Value),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@Quantity", product.Quantity),
                new SqlParameter("@Image", product.Image ?? (object)DBNull.Value),
                new SqlParameter("@Status", product.Status)
            };
            return await _dbHelper.ExecuteProcedureAsync("sp_CreateProduct", parameters);
        }

        public async Task Delete(Guid id)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };
            await _dbHelper.ExecuteProcedureAsync("sp_Deleteproduct", parameters);
            
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                DataTable productsTable = await _dbHelper.ExecuteFunctionAsync("sf_GetAllproducts");

                

                return productsTable.AsEnumerable().Select(row => new Product
                {
                    Id = row.Field<Guid>("Id"),
                    Name = row.Field<string>("Name"),
                    Description = row.Field<string?>("Description"),
                    Price = row.Field<double>("Price"),
                    Quantity = row.Field<int>("Quantity"),
                    Image = row.Field<byte[]?>("Image"),
                    Status = row.Field<int>("Status")
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error bringing products: {ex.Message}");
                throw;
            }
        }

        public async Task<Product> GetById(Guid id)
        {
            try
            {
                
                var parameters = new SqlParameter[]
                {
            new SqlParameter("@Id", id)
                };

                DataTable productTable = await _dbHelper.ExecuteFunctionAsync("sf_GetProduct", parameters);

                if (productTable.Rows.Count == 0)
                    throw new KeyNotFoundException("Product not found");

                return new Product
                {
                    Id = productTable.Rows[0].Field<Guid>("Id"),
                    Name = productTable.Rows[0].Field<string>("Name"),
                    Description = productTable.Rows[0].Field<string?>("Description"),
                    Price = productTable.Rows[0].Field<double>("Price"),
                    Quantity = productTable.Rows[0].Field<int>("Quantity"),
                    Image = productTable.Rows[0].Field<byte[]?>("Image"),
                    Status = productTable.Rows[0].Field<int>("Status")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<int> Update(Product product)
        {
            var parameters = new SqlParameter[]
              {
                new SqlParameter("@Id", product.Id),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Description", product.Description ?? (object)DBNull.Value),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@Quantity", product.Quantity),
                new SqlParameter("@Image", product.Image ?? (object)DBNull.Value),
                new SqlParameter("@Status", product.Status)
              };
            return await _dbHelper.ExecuteProcedureAsync("sp_UpdateProduct", parameters);
        }
    }
}
