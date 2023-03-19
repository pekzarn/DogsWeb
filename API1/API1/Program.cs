using API1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add database support...
builder.Services.AddDbContext<API1.Data.DogsContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Load data into database...
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<API1.Data.DogsContext>();

    await context.Database.MigrateAsync();

    await SeedData.LoadDogData(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the app to use CORS.
app.UseCors("CorsPolicy");

// Configure the app to use routing and controllers.
app.MapControllers();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
