using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities
{
    public class CinemaDiscount: BaseEntity
    {
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        [Precision(precision: 5, scale: 2)]
        public decimal Discount { get; set; }
        public int CinemaId { get; set; }




    }
}
