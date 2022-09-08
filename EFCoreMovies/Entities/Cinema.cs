using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.Entities
{
    public class Cinema: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Precision(precision: 3, scale: 2)]
        public decimal Price { get; set; }  
        [Required]
        public Point Location { get; set; }

    }
}
