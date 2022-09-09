using EFCoreMovies.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//1. Añadir el DbContext de nuestra app
// 1.1  Usamos la variable DefaultConnection de appSettings para setear la cadena de conexión a la BDD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//1.2 Añadir el DbContext al proveedor/contenedor de servicios 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServer => sqlServer.UseNetTopologySuite());

    /* UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
     * -> Establece el comportamiento de seguimiento de las consultas LINQ que se ejecutan en el contexto.
     * NoTracking => Deshabilita el seguimiento de las entidades devueltas
    * -> Mejora el rendimiento/rapidez de consultas
    * -> No se pueden actualizar los datos (tracking deshabilitado)
    * -> Solo para consultas de lectura (no borrado o actualización)
    */
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// Add services to the container.
// Configuramos las opciones de serialización de objetos Json para que ignore los cyclos en objectos anidados
builder.Services.AddControllers().AddJsonOptions(options => 
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Añadimos el servicio de automapper
builder.Services.AddAutoMapper(typeof(Program));

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
