using System.Data.SqlClient;
using System.Data;

public class Department
{
    private readonly Database _database;

    public Department(Database database)
    {
        _database = database;
    }

    public void Insert(string departmentName)
    {
        _database.ProcessQuery(() =>
        {
            var command = new SqlCommand("INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName)");
            command.Parameters.AddWithValue("@DepartmentName", departmentName);
            return command;
        });
    }

    public void Update(int departmentId, string departmentName)
    {
        _database.ProcessQuery(() =>
        {
            var command = new SqlCommand("UPDATE Departments SET DepartmentName = @DepartmentName WHERE DepartmentID = @DepartmentID");
            command.Parameters.AddWithValue("@DepartmentID", departmentId);
            command.Parameters.AddWithValue("@DepartmentName", departmentName);
            return command;
        });
    }

    public void Delete(int departmentId)
    {
        _database.ProcessQuery(() =>
        {
            var command = new SqlCommand("DELETE FROM Departments WHERE DepartmentID = @DepartmentID");
            command.Parameters.AddWithValue("@DepartmentID", departmentId);
            return command;
        });
    }

    public DataTable Search(string departmentName = null)
    {
        return _database.Search(() =>
        {
            var command = new SqlCommand("SELECT * FROM Departments WHERE DepartmentName LIKE @DepartmentName");
            command.Parameters.AddWithValue("@DepartmentName", string.IsNullOrEmpty(departmentName) ? "%" : $"%{departmentName}%");
            return command;
        });
    }
    public void Display()
    {
        var departments = _database.Display("Departments");
        Console.WriteLine("Departments:");
        Console.WriteLine("------------");
        foreach (DataRow row in departments.Rows)
        {
            Console.WriteLine($"{row["DepartmentID"]}\t|\t{row["DepartmentName"]}");
        }
        Console.WriteLine();
    }
}