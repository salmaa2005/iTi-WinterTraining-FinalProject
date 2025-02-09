class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=LUMYY\\SQLEXPRESS;Initial Catalog=Company;Integrated Security=True;TrustServerCertificate=False;";
        var menu = new Menu(connectionString);
        menu.ShowMenu();
    }
}