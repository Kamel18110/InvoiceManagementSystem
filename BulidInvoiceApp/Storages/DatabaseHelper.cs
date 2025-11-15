using Microsoft.Data.SqlClient;
using System.Data;

namespace BulidInvoiceApp.Storages
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        public DatabaseHelper(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<DataTable> ExecuteFunctionAsync(string functionName, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
               
                string query = $"SELECT * FROM {functionName}(";

                if (parameters != null && parameters.Length > 0)
                {
                    var paramNames = parameters.Select(p => p.ParameterName).ToArray();
                    query += string.Join(", ", paramNames);
                }

                query += ")";

                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }
        public DataTable ExecuteFunction(string functionName, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                
                string query = $"SELECT * FROM {functionName}(";

                if (parameters != null && parameters.Length > 0)
                {
                    var paramNames = parameters.Select(p => p.ParameterName).ToArray();
                    query += string.Join(", ", paramNames);
                }

                query += ")";

                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }
        public async Task<int> ExecuteProcedureAsync(string storedProcedure, SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public int ExecuteProcedure(string storedProcedure, SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }
        public async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }


        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
