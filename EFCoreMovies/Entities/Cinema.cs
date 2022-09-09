using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.Entities
{
    public class Cinema: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Point Location { get; set; }
        public CinemaDiscount CinemaDiscount { get; set; }
        // Relacion 1 a N con salas de cine 
        // Diferencia entre Icollection/List y hashSet -> HashSet<> no permite ordenar items los demás si 
        public HashSet<CinemaRoom> CinemaRooms { get; set; }

    }
}
