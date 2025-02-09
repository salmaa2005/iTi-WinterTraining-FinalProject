using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public class Menu
{
    private readonly Database _database;
    private readonly Department _department;
    private readonly Employee _employee;

    public Menu(string connectionString)
    {
        _database = new Database(connectionString);
        _department = new Department(_database);
        _employee = new Employee(_database);
    }

    private int ValidateNumber(string prompt, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (int.TryParse(input, out result) && result >= minValue && result <= maxValue)
            {
                return result;
            }
            else
            {
                Console.WriteLine($"Invalid input. Please enter a Valid Positive Number");
            }
        }
    }

    public int ValidateIdExists(string prompt, string tableName, string idColumnName, int minValue = 1)
    {
        while (true)
        {
            int id = ValidateNumber(prompt, minValue);
            bool exists = _database.CheckIdExistsInTable(tableName, idColumnName, id);

            if (exists)
            {
                return id;
            }
            else
            {
                Console.WriteLine($"ID {id} does not exist in the {tableName} table. Please try again.");
            }
        }
    }

    public static string ValidateString(string prompt, int minLength = 1, int maxLength = int.MaxValue)
    {
        string result;
        while (true)
        {
            Console.Write(prompt);
            result = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(result) &&
                result.Length >= minLength &&
                result.Length <= maxLength &&
                Regex.IsMatch(result, @"^[a-zA-Z\s]+$"))
            {
                return result;
            }
            else
            {
                Console.WriteLine($"Invalid input. Please enter a string and containing only letters and spaces.");
            }
        }
    }
    public void ShowMenu()
    {
        Console.CursorVisible = false;
        string[] menuOptions = { "Insert", "Update", "Delete", "Search", "Display", "Exit" };
        int highlight = 0;

        while (true)
        {
            Console.Clear();
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            int verticalSpacing = screenHeight / (menuOptions.Length + 1);
            int textX = screenWidth / 2;

            for (int i = 0; i < menuOptions.Length; i++)
            {
                int textY = verticalSpacing * (i + 1);
                Console.SetCursorPosition(textX - (menuOptions[i].Length / 2), textY);

                if (i == highlight)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.Write(menuOptions[i]);
                Console.ResetColor();
            }

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    highlight = (highlight == 0) ? menuOptions.Length - 1 : highlight - 1;
                    break;
                case ConsoleKey.DownArrow:
                    highlight = (highlight == menuOptions.Length - 1) ? 0 : highlight + 1;
                    break;
                case ConsoleKey.Enter:
                    HandleMenuSelection(menuOptions[highlight]);
                    break;
            }
        }
    }

    private void HandleMenuSelection(string option)
    {
        Console.Clear();
        Console.WriteLine($"You selected: {option}");

        if (option == "Exit")
        {
            Environment.Exit(0);
        }

        Console.WriteLine("Choose a table to perform the action on:");
        Console.WriteLine("1. Employee");
        Console.WriteLine("2. Department");
        int tableChoice = ValidateNumber("Enter your choice: ", 1, 2);

        switch (option)
        {
            case "Insert":
                if (tableChoice == 1)
                {
                    string employeeName = ValidateString("Enter Employee Name: ", 1, 100);
                    Console.WriteLine("Here are the Departments, Choose An ID To Assign the new employee to:");
                    _department.Display();
                    int departmentId = ValidateIdExists("Enter the department ID: ", "Departments", "DepartmentID");
                    _employee.Insert(employeeName, departmentId);
                }
                else if (tableChoice == 2)
                {
                    string departmentName = ValidateString("Enter Department Name: ", 1, 100);
                    _department.Insert(departmentName);
                }
                break;

            case "Update":
                if (tableChoice == 1)
                {
                    Console.WriteLine("Here are the Employees, Choose an ID To Update:");
                    _employee.Display();
                    int employeeId = ValidateIdExists("Enter Employee ID: ", "Employees", "EmployeeID");
                    string newEmployeeName = ValidateString("Enter new Employee Name: ", 1, 100);
                    Console.WriteLine("Here are the Departments, Choose A New Department ID:");
                    _department.Display();
                    int newDepartmentId = ValidateIdExists("Enter new Department ID: ", "Departments", "DepartmentID");
                    _employee.Update(employeeId, newEmployeeName, newDepartmentId);
                }
                else if (tableChoice == 2)
                {
                    Console.WriteLine("Here are the Departments, Choose A Department ID To Update:");
                    _department.Display();
                    int departmentId = ValidateIdExists("Enter Department ID to update: ", "Departments", "DepartmentID");
                    string newDepartmentName = ValidateString("Enter new Department Name: ", 1, 100);
                    _department.Update(departmentId, newDepartmentName);
                }
                break;

            case "Delete":
                if (tableChoice == 1)
                {
                    Console.WriteLine("Here are the Employees, Choose an ID To Delete:");
                    _employee.Display();
                    int employeeId = ValidateIdExists("Enter Employee ID to delete: ", "Employees", "EmployeeID");
                    _employee.Delete(employeeId);
                }
                else if (tableChoice == 2)
                {
                    Console.WriteLine("Here are the Departments, Choose A Department ID To Delete:");
                    _department.Display();
                    int departmentId = ValidateIdExists("Enter Department ID to delete: ", "Departments", "DepartmentID");
                    _department.Delete(departmentId);
                }
                break;

            case "Search":
                if (tableChoice == 1)
                {
                    string employeeName = ValidateString("Enter Employee Name to search: ", 0, 100);
                    var employees = _employee.Search(employeeName);
                    Console.WriteLine("Search Results:");
                    foreach (DataRow row in employees.Rows)
                    {
                        Console.WriteLine($"EmployeeID: {row["EmployeeID"]}, EmployeeName: {row["EmployeeName"]}, DepartmentID: {row["DepartmentID"]}");
                    }
                }
                else if (tableChoice == 2)
                {
                    string departmentName = ValidateString("Enter Department Name to search: ", 0, 100);
                    var departments = _department.Search(departmentName);
                    Console.WriteLine("Search Results:");
                    foreach (DataRow row in departments.Rows)
                    {
                        Console.WriteLine($"DepartmentID: {row["DepartmentID"]}, DepartmentName: {row["DepartmentName"]}");
                    }
                }
                break;

            case "Display":
                if (tableChoice == 1)
                {
                    _employee.Display();
                }
                else if (tableChoice == 2)
                {
                    _department.Display();
                }
                break;
        }

        Console.WriteLine("Press ENTER to return to the menu...");
        Console.ReadLine();
    }
}