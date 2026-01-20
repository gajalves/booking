using Microsoft.Data.SqlClient;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("[PreBase] ERRO: ConnectionString 'DefaultConnection' não encontrada!");
    Environment.Exit(1);
}

var masterConnectionString = connectionString.Replace("Database=booking", "Database=master");
const int maxRetries = 10;
const int delaySeconds = 3;

for (int attempt = 1; attempt <= maxRetries; attempt++)
{
    try
    {
        using var connection = new SqlConnection(masterConnectionString);
        connection.Open();

        // Cria o database "booking" se não existir
        var createDbCommand = connection.CreateCommand();
        createDbCommand.CommandText = @"
            IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'booking')
            BEGIN
                CREATE DATABASE [booking];
            END";
        
        createDbCommand.ExecuteNonQuery();
        connection.Close();

        // Aguarda database ficar disponível
        Thread.Sleep(TimeSpan.FromSeconds(2));

        // Testa conexão no database "booking"
        using var bookingConnection = new SqlConnection(connectionString);
        bookingConnection.Open();
        var testCommand = bookingConnection.CreateCommand();
        testCommand.CommandText = "SELECT 1";
        testCommand.ExecuteScalar();
        bookingConnection.Close();

        Console.WriteLine("[PreBase] Database 'booking' criado e verificado com sucesso!");
        Environment.Exit(0);
    }
    catch (Exception ex)
    {
        if (attempt >= maxRetries)
        {
            Console.WriteLine($"[PreBase] FALHA após {maxRetries} tentativas: {ex.Message}");
            Environment.Exit(1);
        }
        
        Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
    }
}
