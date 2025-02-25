using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooKing.Generics.Infra;
public static class Migration
{
    private static readonly int maxAttempts = 5;
    static int attempt = 0;

    public static void RunMigration<T>(this IServiceProvider app) where T : DbContext
    {
        using (var serviceScope = app.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<T>();
            context.Database.SetCommandTimeout(3 * 1000);

            var delay = TimeSpan.FromSeconds(5);

            while (attempt < maxAttempts)
            {
                try
                {
                    if (context.Database.CanConnect())
                    {
                        if (context.Database.GetPendingMigrations().Any())
                        {
                            context.Database.Migrate();
                        }
                        return;
                    }
                }
                catch
                {
                    attempt++;
                    if (attempt == maxAttempts)
                        throw new Exception("Não foi possível conectar ao banco de dados após várias tentativas.");

                    Thread.Sleep(delay);
                }
            }
        }
    }
}
