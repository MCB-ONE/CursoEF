using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.DTOs.Cinema
{
    public class CinemaDiscountCreateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Range(1, 100)]
        public decimal Discount { get; set; }
    }
}
