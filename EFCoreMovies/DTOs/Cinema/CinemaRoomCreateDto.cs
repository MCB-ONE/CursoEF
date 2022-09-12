using EFCoreMovies.Entities;

namespace EFCoreMovies.DTOs.Cinema
{
    public class CinemaRoomCreateDto
    {
        public decimal Price { get; set; }
        public RoomType RoomType { get; set; }
    }
}
