using System.Data.SqlClient;
using System.Data;

public class Employee
{
    private readonly Database _database;

    public Employee(Database database)
    {
        _database = database;
    }

    public void Insert(string employeeName, int departmentId)
    {
        _database.ProcessQuery(() =>
        {
            var command = new SqlCommand("INSERT INTO Employees (EmployeeName, DepartmentID) VALUES (@EmployeeName, @DepartmentID)");
            command.Parameters.AddWithValue("@EmployeeName", employeeName);
            command.Parameters.AddWithValue("@DepartmentID", departmentId);
            return command;
        });
    }

    public void Update(int employeeId, string employeeName, int departmentId)
    {
        _database.ProcessQuery(() =>
        {
            var command = new SqlCommand("UPDATE Employees SET EmployeeName = @EmployeeName, DepartmentID = @DepartmentID WHERE EmployeeID = @EmployeeID");
            command.Parameters.AddWithValue("@EmployeeID", employeeId);
            command.Parameters.AddWithValue("@EmployeeName", employeeName);
            command.Parameters.AddWithValue("@DepartmentID", departmentId);
            return command;
        });
    }

    public void Delete(int employeeId)
    {
        _database.ProcessQuery(() =>
        {
            var command = new SqlCommand("DELETE FROM Employees WHERE EmployeeID = @EmployeeID");
            command.Parameters.AddWithValue("@EmployeeID", employeeId);
            return command;
        });
    }

    public DataTable Search(string employeeName = null)
    {
        return _database.Search(() =>
        {
            var command = new SqlCommand("SELECT * FROM Employees WHERE EmployeeName LIKE @EmployeeName");
            command.Parameters.AddWithValue("@EmployeeName", string.IsNullOrEmpty(employeeName) ? "%" : $"%{employeeName}%");
            return command;
        });
    }
    public void Display()
    {
        var employees = _database.Display("Employees");
        Console.WriteLine("Employees:");
        Console.WriteLine("----------");
        foreach (DataRow row in employees.Rows)
        {
            string departmentId = row.IsNull("DepartmentID") ? "NULL" : row["DepartmentID"].ToString();
            Console.WriteLine($"{row["EmployeeID"]}\t|\t{row["EmployeeName".PadRight(4)]}\t|\t{departmentId}");
        }
        Console.WriteLine();
    }
}