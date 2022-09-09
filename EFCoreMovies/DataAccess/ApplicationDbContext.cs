using EFCoreMovies.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFCoreMovies.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*ApplyConfigurationsFromAssembly("ensamlado") 
                => escanea el proyecto (el ensamblado) y aplica a api fluent todas aquellas clases que implementan IEntityTypeConfiguration (configuración de entidades)
                => le hemos de indicar el ensamblado que contenga las configuraciones
             (Assembly.GetExecutingAssembly()) => Permite pasar por parámetro el ensablado que se esta ejecyutando 
             */
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<CinemaDiscount> CinemaDiscounts { get; set; }
        public DbSet<CinemaRoom> CinemaRooms { get; set; }
        public DbSet<MovieActor> MoviesActors { get; set; }

    }
}
