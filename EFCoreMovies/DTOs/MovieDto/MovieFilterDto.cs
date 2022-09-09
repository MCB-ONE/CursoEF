namespace EFCoreMovies.DTOs.MovieDto
{
    public class MovieFilterDto
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool NextPremiere { get; set; }
        public bool IsProjected { get; set; }

    }
}
