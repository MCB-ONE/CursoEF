using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.Entities
{
    public class Genre: BaseEntity
    {
        public string Name { get; set; }    
        public bool IsDeleted { get; set; }
        public HashSet<Movie> Movies { get; set; }

    }
}
