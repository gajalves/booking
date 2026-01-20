using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooKing.Generics.Infra;
public static class Migration
{
    private static readonly int maxAttempts = 5;
    private static readonly TimeSpan delay = TimeSpan.FromSeconds(2);

    public static void RunMigration<T>(this IServiceProvider app) where T : DbContext
    {
        using (var serviceScope = app.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<T>();
            
            if (context == null)
            {
                throw new Exception("DbContext não foi encontrado no ServiceProvider");
            }
            
            var contextType = typeof(T).Name;
            Console.WriteLine($"[Migration] Iniciando migrations para: {contextType}");

            context.Database.SetCommandTimeout(30);

            int attempt = 0;

            while (attempt < maxAttempts)
            {
                try
                {
                    if (context.Database.CanConnect())
                    {
                        var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                        if (pendingMigrations.Any())
                        {
                            Console.WriteLine($"[Migration] Aplicando {pendingMigrations.Count} migração(ões)...");
                            context.Database.Migrate();
                            Console.WriteLine("[Migration] Migrações aplicadas com sucesso.");
                        }
                        else
                        {
                            Console.WriteLine("[Migration] Nenhuma migração pendente.");
                        }
                        return;
                    }
                    
                    attempt++;
                    Thread.Sleep(delay);
                }
                catch (Exception ex)
                {
                    attempt++;
                    
                    if (attempt >= maxAttempts)
                    {
                        Console.WriteLine($"[Migration] Falha após {maxAttempts} tentativas: {ex.Message}");
                        throw new Exception("Não foi possível conectar ao banco de dados após várias tentativas.", ex);
                    }

                    Thread.Sleep(delay);
                }
            }
        }
    }
}
