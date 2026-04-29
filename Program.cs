using Microsoft.EntityFrameworkCore;
using Reminders.Data;

var builder = WebApplication.CreateBuilder(args);

var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var user = Environment.GetEnvironmentVariable("DB_USER");
var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
var name = Environment.GetEnvironmentVariable("DB_NAME");
var connectionString = $"Host={host};Port={port};Username={user};Password={pass};Database={name};";

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.ExecuteSqlRawAsync(@"
        CREATE TABLE IF NOT EXISTS ""Reminders"" (
            ""Id""        SERIAL PRIMARY KEY,
            ""Title""     VARCHAR(200) NOT NULL,
            ""Notes""     TEXT,
            ""DueAt""     TIMESTAMPTZ,
            ""Completed"" BOOLEAN NOT NULL DEFAULT FALSE,
            ""CreatedAt"" TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
        );
    ");
}

app.UseStaticFiles();
app.MapRazorPages();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
