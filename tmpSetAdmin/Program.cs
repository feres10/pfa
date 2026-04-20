using System;
using Npgsql;
using BCrypt.Net;
class Program
{
    static int Main(string[] args)
    {
        var pwd = args.Length > 0 ? args[0] : "admin";
        var connString = "Host=localhost;Port=5432;Database=EHealthDb;Username=postgres;Password=root";
        var hash = BCrypt.Net.BCrypt.HashPassword(pwd);
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE \"Comptes\" SET \"MotDePasse\" = @hash WHERE \"Email\" = @email";
        cmd.Parameters.AddWithValue("hash", hash);
        cmd.Parameters.AddWithValue("email", "admin@localhost");
        var rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"Updated rows: {rows}");
        Console.WriteLine(hash);
        return 0;
    }
}
