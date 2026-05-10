# Kuva.Auth EF Migrations

Use este projeto para gerar e aplicar migrations do Auth Service sem acoplar o histórico de banco ao runtime da API.

```bash
dotnet ef migrations add InitialCreate \
  --project Source/Kuva.Auth.EFMigrations \
  --startup-project Source/Kuva.Auth.Service \
  --context AuthDbContext

dotnet ef database update \
  --project Source/Kuva.Auth.EFMigrations \
  --startup-project Source/Kuva.Auth.Service \
  --context AuthDbContext
```

Connection string local:

```bash
export ConnectionStrings__AuthDatabase="Server=localhost,1433;Database=KuvaAuth;User Id=sa;Password=Your_strong_password123;TrustServerCertificate=True"
```
