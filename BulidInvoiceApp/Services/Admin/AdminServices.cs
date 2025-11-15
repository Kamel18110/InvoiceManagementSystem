using BulidInvoiceApp.Models;
using BulidInvoiceApp.Storages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BulidInvoiceApp.Services.Admin
{
    public class AdminServices : IadminSrevices
    {
        private readonly DatabaseHelper _dbHelper;

        public AdminServices(DatabaseHelper dbHelper)
        {
            this._dbHelper = dbHelper;
        }

        public async Task<CompanyInfo?> GetCompanyInfoAsync()
        {
            try
            {
                var query = @"
                    SELECT TOP 1 id, Name, Logo, Address, phone, email 
                    FROM [dbo].[ADMIN] 
                    ORDER BY Name";

                var result = await _dbHelper.ExecuteQueryAsync(query);

                if (result.Rows.Count > 0)
                {
                    var row = result.Rows[0];
                    return new CompanyInfo
                    {
                        Id = row.Field<Guid>("id"),
                        Name = row.Field<string>("Name") ?? string.Empty,
                        Logo = row.Field<byte[]?>("Logo"),
                        Address = row.Field<string>("Address") ?? string.Empty,
                        Phone = row.Field<string>("phone") ?? string.Empty,
                        Email = row.Field<string>("email") ?? string.Empty
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting company info: {ex.Message}");
                return null;
            }
        }

        public async Task SaveCompanyInfoAsync(CompanyInfo companyInfo)
        {
            try
            {
                var parameters = new[]
                {

                    new SqlParameter("@Id", companyInfo.Id) ,
                    new SqlParameter("@Name",companyInfo.Name) ,
                    new SqlParameter("@Logo",(object?)companyInfo.Logo ?? DBNull.Value ),
                    new SqlParameter("@Address", companyInfo.Address) ,
                    new SqlParameter("@Phone",  companyInfo.Phone) ,
                    new SqlParameter("@Email",companyInfo.Email )
                };

                var query = @"
                    INSERT INTO [dbo].[ADMIN] (Id, Name, Logo, Address, phone, email)
                    VALUES (@Id, @Name, @Logo, @Address, @Phone, @Email)";

                 await _dbHelper.ExecuteQueryAsync(query, parameters);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving company info: {ex.Message}");
               
            }
        }

        public async Task UpdateCompanyInfoAsync(CompanyInfo companyInfo)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Id", companyInfo.Id) ,
                    new SqlParameter("@Name",companyInfo.Name) ,
                    new SqlParameter("@Logo",(object?)companyInfo.Logo ?? DBNull.Value ),
                    new SqlParameter("@Address", companyInfo.Address) ,
                    new SqlParameter("@Phone",  companyInfo.Phone) ,
                    new SqlParameter("@Email",companyInfo.Email ) 
                };

                var query = @"
                    UPDATE [dbo].[ADMIN] 
                    SET Name = @Name, 
                        Logo = @Logo, 
                        Address = @Address, 
                        phone = @Phone, 
                        email = @Email 
                    WHERE Id = @Id";

                await _dbHelper.ExecuteQueryAsync(query, parameters);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating company info: {ex.Message}");
              
            }
        }
    }
}