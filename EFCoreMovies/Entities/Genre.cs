using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.Entities
{
    public class Genre: BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }    
    }
}
