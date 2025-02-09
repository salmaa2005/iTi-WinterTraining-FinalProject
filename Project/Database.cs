using System;
using System.Data;
using System.Data.SqlClient;

public class Database
{
    public readonly string _connectionString;

    public Database(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void ProcessQuery(Func<SqlCommand> commandCreator)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = commandCreator())
            {
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }
    }

    public DataTable Search(Func<SqlCommand> commandCreator)
    {
        var dataTable = new DataTable();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = commandCreator())
            {
                command.Connection = connection;
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }
        }
        return dataTable;
    }
    public DataTable Display(string tableName)
    {
        
        var dataTable = new DataTable();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
            {
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }
        }
        return dataTable;
    }
    public bool CheckIdExistsInTable(string tableName, string idColumnName, int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand($"SELECT COUNT(*) FROM {tableName} WHERE {idColumnName} = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }
}