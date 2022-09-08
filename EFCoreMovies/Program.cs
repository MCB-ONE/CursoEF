using EFCoreMovies.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//1. Añadir el DbContext de nuestra app
// 1.1  Usamos la variable DefaultConnection de appSettings para setear la cadena de conexión a la BDD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//1.2 Añadir el DbContext al proveedor/contenedor de servicios 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
    sqlServer => sqlServer.UseNetTopologySuite()));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
