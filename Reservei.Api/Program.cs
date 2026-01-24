using Reservei.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Aspire Defaults
builder.AddServiceDefaults();

// 2. Configuration Extensions (DI Container)
builder.AddInfrastructure();
builder.Services.AddApplicationServices();
builder.Services.AddApiDocumentation();
builder.Services.AddApiErrorHandling();
builder.Services.AddControllers();

var app = builder.Build();

app.UseApiErrorHandling();
await app.ApplyMigrationsAsync();
app.UseApiDocumentation();
app.MapDefaultEndpoints();
app.MapProjectEndpoints();

app.Run();
