using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities
{
    public class Movie: BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsProyected { get; set; }
        [Column(TypeName = "Date")]
        public DateTime PremiereDate { get; set; }
        [MaxLength(500)]
        [Unicode(false)]
        public string PosterUrl { get; set; }

        // AUTOMATIC Many to many relationships: Tabla intermedia creada por ef
        //Hemos de usar List no HashSet (ya que hashSet no es un  IEnumerable)
        public List<Genre> Genres { get; set; } 
        public List<CinemaRoom> CinemaRooms { get; set; }

        // MANUAL Many to many relationship: Usando entidad intermedia para acceder a la tabla de relación
        public List<MovieActor> MoviesActors { get; set; }

    }
}
