using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.Entities
{
    public class Genre: BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }    
        public HashSet<Movie> Movies { get; set; }

    }
}
