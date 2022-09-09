using EFCoreMovies.DTOs.Actor;
using EFCoreMovies.DTOs.Cinema;
using EFCoreMovies.DTOs.Genre;
using EFCoreMovies.Entities;

namespace EFCoreMovies.DTOs.MovieDto
{
    public class MovieDto: BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsProyected { get; set; }
        public DateTime PremiereDate { get; set; }
        public string PosterUrl { get; set; }

        // AUTOMATIC Many to many relationships: Tabla intermedia creada por ef
        public ICollection<GenreDto> Genres { get; set; }
        public ICollection<CinemaDto> Cinemas { get; set; }

        // MANUAL Many to many relationship: Usando entidad intermedia para acceder a la tabla de relación
        public HashSet<ActorDto> Actors { get; set; }

    }
}
