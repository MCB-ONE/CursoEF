using EFCoreMovies.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//1. A�adir el DbContext de nuestra app
// 1.1  Usamos la variable DefaultConnection de appSettings para setear la cadena de conexi�n a la BDD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//1.2 A�adir el DbContext al proveedor/contenedor de servicios 
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
