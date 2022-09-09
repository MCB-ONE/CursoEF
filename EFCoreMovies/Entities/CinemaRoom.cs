using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.Entities
{
    public enum RoomType
    {
        TwoDimensions = 1,
        ThreeDimensions = 2,
        Vip = 3
    } 
    public class CinemaRoom: BaseEntity
    {
        [Required]
        [Precision(precision: 3, scale: 2)]
        public decimal Price { get; set; }
        [Required]
        public RoomType RoomType { get; set; }
        public int CinemaId { get; set; } 
        public Cinema Cinema { get; set; }

        public HashSet<Movie> Movies { get; set; }

    }
}
