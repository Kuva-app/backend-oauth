using Kuva.Auth.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Kuva.Auth.EFMigrations;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("../Kuva.Auth.Service/appsettings.json", optional: true)
            .AddJsonFile("../Kuva.Auth.Service/appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("AuthDatabase")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__AuthDatabase")
            ?? "Server=tcp:localhost,1433;Persist Security Info=False;User ID=sa;Password=Change_this_password_123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;Database=KuvaAuth";

        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName))
            .Options;

        return new AuthDbContext(options);
    }
}
