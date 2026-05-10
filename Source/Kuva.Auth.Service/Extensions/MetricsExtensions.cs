using Prometheus;

namespace Kuva.Auth.Service.Extensions;

public static class MetricsExtensions
{
    public static readonly Counter RequestsTotal = Metrics.CreateCounter("kuva_auth_requests_total", "Total de requisições HTTP.", ["endpoint", "method", "status_code"]);
    public static readonly Histogram RequestDuration = Metrics.CreateHistogram("kuva_auth_request_duration_seconds", "Duração das requisições HTTP.", ["endpoint", "method"]);
    public static readonly Counter LoginSuccessTotal = Metrics.CreateCounter("kuva_auth_login_success_total", "Logins concluídos com sucesso.", ["role"]);
    public static readonly Counter LoginFailedTotal = Metrics.CreateCounter("kuva_auth_login_failed_total", "Falhas de login.", ["failure_reason"]);
    public static readonly Counter RegisterSuccessTotal = Metrics.CreateCounter("kuva_auth_register_success_total", "Cadastros concluídos com sucesso.");
    public static readonly Counter RegisterFailedTotal = Metrics.CreateCounter("kuva_auth_register_failed_total", "Falhas de cadastro.", ["failure_reason"]);
    public static readonly Counter RefreshSuccessTotal = Metrics.CreateCounter("kuva_auth_refresh_success_total", "Refresh token concluído com sucesso.");
    public static readonly Counter RefreshFailedTotal = Metrics.CreateCounter("kuva_auth_refresh_failed_total", "Falhas de refresh token.", ["failure_reason"]);
    public static readonly Counter LogoutTotal = Metrics.CreateCounter("kuva_auth_logout_total", "Logouts realizados.");
    public static readonly Counter TokenIssuedTotal = Metrics.CreateCounter("kuva_auth_token_issued_total", "Tokens emitidos.", ["role"]);
    public static readonly Counter TokenRevokedTotal = Metrics.CreateCounter("kuva_auth_token_revoked_total", "Tokens revogados.");
    public static readonly Counter BlockedUserLoginAttemptTotal = Metrics.CreateCounter("kuva_auth_blocked_user_login_attempt_total", "Tentativas de login de usuários bloqueados.");
    public static readonly Counter StoreOperatorLoginTotal = Metrics.CreateCounter("kuva_auth_store_operator_login_total", "Logins de operadores de loja.");
    public static readonly Histogram DatabaseOperationDuration = Metrics.CreateHistogram("kuva_auth_database_operation_duration_seconds", "Duração de operações de banco.");
    public static readonly Counter KeyVaultFailuresTotal = Metrics.CreateCounter("kuva_auth_keyvault_failures_total", "Falhas de Key Vault.");

    public static IServiceCollection AddAuthMetrics(this IServiceCollection services) => services;

    public static IApplicationBuilder UseAuthMetrics(this IApplicationBuilder app)
    {
        app.UseHttpMetrics(options =>
        {
            options.AddCustomLabel("endpoint", context => context.GetEndpoint()?.DisplayName ?? "unknown");
        });
        app.UseMetricServer("/metrics");
        return app;
    }
}
