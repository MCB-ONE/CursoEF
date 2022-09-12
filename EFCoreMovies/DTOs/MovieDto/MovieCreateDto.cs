using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.DTOs.MovieDto
{
    public class MovieCreateDto
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
        //[MaxLength(500)]
        //[Unicode(false)]
        //public string PosterUrl { get; set; }

        // Crear entidades relacionadas con registros exitentes
        public List<int> Genres { get; set; }
        public List<int> CinemaRooms { get; set; }
        public List<MovieActorCreateDto> MoviesActors { get; set; }
    }
}
