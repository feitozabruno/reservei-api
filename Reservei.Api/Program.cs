using Reservei.Api.Data;
using Reservei.Api.Entities;
using Reservei.Api.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<AppDbContext>("sqldb");

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapIdentityApi<User>()
    .WithTags("Autenticação");

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
