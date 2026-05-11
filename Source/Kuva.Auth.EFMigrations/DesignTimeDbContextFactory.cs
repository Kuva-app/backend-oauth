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
            ?? "Server=localhost,1433;Database=KuvaAuth;User Id=sa;Password=Your_strong_password123;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName))
            .Options;

        return new AuthDbContext(options);
    }
}
