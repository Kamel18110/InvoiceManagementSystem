using BulidInvoiceApp.Models;
using BulidInvoiceApp.Storages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BulidInvoiceApp.Services.Invoices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly DatabaseHelper _dbHelper;

        public InvoiceService(DatabaseHelper dbHelper)
        {
            this._dbHelper = dbHelper;
        }

        public async Task<int> Create(Invoice invoice)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", invoice.Id),
                new SqlParameter("@Date", invoice.Date),
                new SqlParameter("@Tax", invoice.Tax),
                new SqlParameter("@Discount", invoice.Discount),
                new SqlParameter("@NameCustomer", invoice.NameCustomer),
                new SqlParameter("@AddressCustomer", invoice.AddressCustomer ?? (object)DBNull.Value),
                new SqlParameter("@PhoneNumber", invoice.PhoneNumber),
                new SqlParameter("@Status", invoice.Status)
            };
            return await _dbHelper.ExecuteProcedureAsync("sp_CreateInvoice", parameters);
        }

        public async Task<int> CreateInvoiceDetail(InvoiceDetail detail)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", detail.Id),
                new SqlParameter("@InvoiceID", detail.InvoiceID),
                new SqlParameter("@ProductID", detail.ProductID),
                new SqlParameter("@Quantity", detail.Quantity)
            };
            return await _dbHelper.ExecuteProcedureAsync("sp_CreateInvoiceDetail", parameters);
        }

        public async Task<bool> Delete(Guid id)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@aId", id)
            };
            int affectedRows = await _dbHelper.ExecuteProcedureAsync("sp_DeleteInvoice", parameters);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteInvoiceDetail(Guid id)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };
            int affectedRows = await _dbHelper.ExecuteProcedureAsync("sp_DeleteInvoiceDetail", parameters);
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Invoice>> GetAll()
        {
            try
            {
                DataTable invoicesTable = await _dbHelper.ExecuteFunctionAsync("sf_GetAllInvoices");

                return invoicesTable.AsEnumerable().Select(row => new Invoice
                {
                    Id = row.Field<Guid>("Id"),
                    Date = row.Field<DateTime>("Date"),
                    Tax = row.Field<decimal>("Tax"),
                    Discount = row.Field<decimal>("Discount"),
                    NameCustomer = row.Field<string>("NameCustomer"),
                    AddressCustomer = row.Field<string?>("AddressCustomer"),
                    PhoneNumber = row.Field<string>("PhoneNumber"),
                    Status = row.Field<int>("Status")
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error bringing invoices: {ex.Message}");
                throw;
            }
        }

        public async Task<Invoice> GetById(Guid id)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@aId", id)
                };

                DataTable invoiceTable = await _dbHelper.ExecuteFunctionAsync("sf_GetInvoice", parameters);

                if (invoiceTable.Rows.Count == 0)
                    throw new KeyNotFoundException("Invoice not found");

                var invoice = new Invoice
                {
                    Id = invoiceTable.Rows[0].Field<Guid>("Id"),
                    Date = invoiceTable.Rows[0].Field<DateTime>("Date"),
                    Tax = invoiceTable.Rows[0].Field<decimal>("Tax"),
                    Discount = invoiceTable.Rows[0].Field<decimal>("Discount"),
                    NameCustomer = invoiceTable.Rows[0].Field<string>("NameCustomer"),
                    AddressCustomer = invoiceTable.Rows[0].Field<string?>("AddressCustomer"),
                    PhoneNumber = invoiceTable.Rows[0].Field<string>("PhoneNumber"),
                    Status = invoiceTable.Rows[0].Field<int>("Status")
                };

                invoice.Details = (await GetInvoiceDetails(id)).ToList();
                return invoice;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting invoice by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<InvoiceDetail>> GetInvoiceDetails(Guid invoiceId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@InvoiceID", invoiceId)
                };

                DataTable detailsTable = await _dbHelper.ExecuteFunctionAsync("sF_GetInvoiceDetailsByInvoiceID", parameters);

                return detailsTable.AsEnumerable().Select(row => new InvoiceDetail
                {
                    Id = row.Field<Guid>("Id"),
                    InvoiceID = row.Field<Guid>("InvoiceID"),
                    ProductID = row.Field<Guid>("ProductID"),
                    Quantity = row.Field<int>("Quantity")
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting invoice details: {ex.Message}");
                throw;
            }
        }

        public async Task<int> Update(Invoice invoice)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", invoice.Id),
                new SqlParameter("@Date", invoice.Date),
                new SqlParameter("@Tax", invoice.Tax),
                new SqlParameter("@Discount", invoice.Discount),
                new SqlParameter("@NameCustomer", invoice.NameCustomer),
                new SqlParameter("@AddressCustomer", invoice.AddressCustomer ?? (object)DBNull.Value),
                new SqlParameter("@PhoneNumber", invoice.PhoneNumber),
                new SqlParameter("@Status", invoice.Status)
            };
            return await _dbHelper.ExecuteProcedureAsync("sp_UpdateInvoice", parameters);
        }
    }
}