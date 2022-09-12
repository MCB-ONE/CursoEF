using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.DTOs.MovieDto
{
    public class MovieActorCreateDto
    {

        [Required]
        public int ActorId { get; set; }
        [Required]
        public string Character { get; set; }

    }
}
