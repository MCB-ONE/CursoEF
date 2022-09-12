using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.DTOs.Cinema
{
    public class CinemaCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }
        public CinemaDiscountCreateDto CinemaDiscount { get; set; }

        public CinemaRoomCreateDto[] cinemaRooms { get; set; }

}
}
