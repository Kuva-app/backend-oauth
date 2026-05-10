# Kuva.Auth

## Objetivo

Microsserviço de autenticação e autorização da plataforma Kuva. Ele cobre cadastro, login, JWT, JWKS, refresh token com rotação, logout, roles, permissões, vínculo operador-loja, auditoria básica, health checks e métricas Prometheus.

## Arquitetura

- `Kuva.Auth.Entities`: domínio compartilhado, DTOs, enums, constantes e value objects.
- `Kuva.Auth.Repository`: `AuthDbContext`, Fluent API, seed inicial, repositórios e unit of work.
- `Kuva.Auth.Business`: regras de autenticação, hashing, JWT, JWKS, refresh token e operadores.
- `Kuva.Auth.Service`: API ASP.NET Core, middlewares, Swagger, auth bearer, policies, health e metrics.
- `Kuva.Auth.EFMigrations`: migrations isoladas do Entity Framework Core.
- `Kuva.Auth.Tests`: testes unitários com NUnit, Moq, FluentAssertions e EF InMemory.

## Requisitos

- .NET SDK 10.
- Docker e Docker Compose para ambiente local completo.
- SQL Server local ou container.

## Rodando local

```bash
dotnet restore Source/Kuva.Auth.sln
dotnet build Source/Kuva.Auth.sln -m:1
dotnet test Source/Kuva.Auth.sln -m:1
dotnet run --project Source/Kuva.Auth.Service
```

> Use `-m:1` neste ambiente quando o MSBuild paralelo travar na resolução de referências entre projetos.

Ambiente Docker:

```bash
docker compose up --build
```

- API: `http://localhost:8081`
- Swagger: `http://localhost:8081/swagger`
- Health: `http://localhost:8081/health`
- Metrics: `http://localhost:8081/metrics`
- Prometheus: `http://localhost:9090`
- Grafana: `http://localhost:3000`

## Variáveis de ambiente

```text
ASPNETCORE_ENVIRONMENT
ASPNETCORE_URLS
ConnectionStrings__AuthDatabase
KeyVault__Uri
Jwt__Issuer
Jwt__Audience
Jwt__AccessTokenMinutes
Jwt__KeyId
Jwt__PrivateKeySecretName
RefreshToken__ConsumerExpirationDays
RefreshToken__MerchantExpirationHours
PasswordPolicy__MinimumLength
Cors__AllowedOrigins__0
```

## User Secrets

```bash
dotnet user-secrets set "ConnectionStrings:AuthDatabase" "Server=localhost,1433;Database=KuvaAuth;User Id=sa;Password=Your_strong_password123;TrustServerCertificate=True" --project Source/Kuva.Auth.Service
dotnet user-secrets set "Jwt:PrivateKeyPem" "<PEM_PRIVADO>" --project Source/Kuva.Auth.Service
```

## Azure Key Vault

Segredos esperados:

- `ConnectionStrings--AuthDatabase`
- `Jwt--PrivateKeyPem`
- `Jwt--PublicKeyPem`
- `Jwt--KeyId`

Em produção, use Managed Identity e configure `KeyVault__Uri`.

## Migrations

```bash
dotnet tool restore
dotnet tool run dotnet-ef migrations add InitialCreate \
  --project Source/Kuva.Auth.EFMigrations \
  --startup-project Source/Kuva.Auth.EFMigrations \
  --context AuthDbContext \
  --output-dir Migrations

dotnet tool run dotnet-ef database update \
  --project Source/Kuva.Auth.EFMigrations \
  --startup-project Source/Kuva.Auth.EFMigrations \
  --context AuthDbContext
```

## Endpoints

- `POST /api/v1/auth/register`
- `POST /api/v1/auth/login`
- `POST /api/v1/auth/refresh-token`
- `POST /api/v1/auth/logout`
- `GET /api/v1/auth/me`
- `GET /api/v1/auth/jwks`
- `POST /api/v1/auth/internal/stores/{storeId}/operators`
- `GET /api/v1/auth/internal/stores/{storeId}/operators`
- `PATCH /api/v1/auth/internal/operators/{operatorId}/status`

## Observabilidade

- `/health`
- `/health/live`
- `/health/ready`
- `/metrics`

O `docker-compose.yml` sobe Prometheus e Grafana com scrape de `kuva-auth-service:8080/metrics`.

## Testes

```bash
dotnet test Source/Kuva.Auth.sln -m:1
dotnet test Source/Kuva.Auth.sln --collect:"XPlat Code Coverage" --results-directory TestResults -m:1
```

## Segurança

- Senhas usam `PasswordHasher<TUser>`.
- Refresh tokens são retornados apenas uma vez e persistidos como SHA-256.
- Refresh token é rotacionado e o token anterior é revogado.
- JWT usa RSA e expõe chave pública via JWKS.
- Auditoria evita registrar senha, access token, refresh token e chave privada.

## Troubleshooting

- Se `dotnet test` falhar com `SocketException (13): Permission denied`, execute fora do sandbox local.
- Se o build paralelo travar sem erros, use `-m:1`.
- Se `dotnet ef` não existir, rode `dotnet tool restore`.
