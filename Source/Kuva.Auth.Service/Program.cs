using Kuva.Auth.Business.Extensions;
using Kuva.Auth.Repository.Extensions;
using Kuva.Auth.Service.Extensions;
using Kuva.Auth.Service.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAuthKeyVault(builder.Configuration);
builder.Services.AddAuthApi(builder.Configuration);
builder.Services.AddAuthRepository(builder.Configuration);
builder.Services.AddAuthBusiness(builder.Configuration);
builder.Services.AddAuthAuthentication(builder.Configuration);
builder.Services.AddAuthAuthorization();
builder.Services.AddAuthHealthChecks(builder.Configuration);
builder.Services.AddAuthSwagger();
builder.Services.AddAuthMetrics();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseAuthSwagger();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthMetrics();
app.MapControllers();
app.MapAuthHealthChecks();

app.Run();

public partial class Program;
