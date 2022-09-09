using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DataAccess;
using EFCoreMovies.DTOs.Cinema;
using EFCoreMovies.DTOs.MovieDto;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MoviesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Eager loading
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDto>> Get(int id)
        {
            var movie = await _context.Movies
                .Include(mov => mov.Genres)
                .Include(mov => mov.CinemaRooms)
                    .ThenInclude(room => room.Cinema)
                .Include(mov => mov.MoviesActors)
                    .ThenInclude(movActor => movActor.Actor)
                .FirstOrDefaultAsync(mov => mov.Id == id);

            if (movie is null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);

            // Eliminamos los registros de cine repetidos
            movieDto.Cinemas = movieDto.Cinemas.DistinctBy(c => c.Id).ToList();

            return Ok(movieDto);

        }

        // Carga de entidades relacionadas usando select()
        [HttpGet("selectiveLoading/{id:int}")]
        public async Task<ActionResult> GetSelective(int id)
        {
            var movie = await _context.Movies.Select(mov =>
            new
            {
                Id = mov.Id,
                Title = mov.Title,
                Genres = mov.Genres.Select( g => g.Name).ToList(),
                ActorsQuantity = mov.MoviesActors.Count(),
                CinemaProjecting = mov.CinemaRooms.Select(cr => cr.CinemaId).Distinct().Count(),
            }).FirstOrDefaultAsync(mov => mov.Id == id);

            if (movie is null)
            {
                return NotFound();
            }


            return Ok(movie);
        }

        /* Carga explícita de entidades relacionadas 
         *  -> Priemro se consulta la entidad principal y despés se cargan las entidades relacionadas que se necesiten
         *  */
        [HttpGet("explicitLoading/{id:int}")]
        public async Task<ActionResult<MovieDto>> GetExplicitLoading(int id)
        {
            // Es necesario activar el traqueo de la entidad para realizar la carga expliccita
            var movie = await _context.Movies.AsTracking().FirstOrDefaultAsync(mov => mov.Id == id);

            //..... Acciones sobre la entidad.....//

            // Cargar entidad relacionada
            await _context.Entry(movie).Collection(mov => mov.Genres).LoadAsync();

            if (movie is null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);


            return Ok(movieDto);
        }

        [HttpGet("groupByProjected")]
        public async Task<ActionResult> GetGroupByProjected()
        {
            var groupedMovies = await _context.Movies.GroupBy(mov => mov.IsProyected)
                                .Select(g => new
                                {
                                    IsProjected = g.Key,
                                    Quantity = g.Count(),
                                    Movies = g.ToList()
                                }).ToListAsync();

            return Ok(groupedMovies);
        }

        [HttpGet("groupByGenreQuanity")]
        public async Task<ActionResult> GetGroupByGenreQuantity()
        {
            var groupedMovies = await _context.Movies.GroupBy(mov => mov.Genres.Count())
                                .Select(g => new
                                {
                                    Quantity = g.Key,
                                    Titles = g.Select(x => x.Title).ToList(),
                                    Genres = g.Select(x => x.Genres)
                                            .SelectMany(gen => gen).Select( g => g.Name).Distinct()
                                }).ToListAsync();

            return Ok(groupedMovies);
        }


        // Uso de ejecución diferida para aplicar filtros dinámicos
        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDto>>> Filter([FromQuery] MovieFilterDto movieFilterDto )
        {
            /*AsQueryable() 
             * => tipo de dato que permite contruir queries 
             * => permite guardar querie strings en diferido (durante la ejecución) para lanzar la consulta final cuando dessemos
            */
            var movieQueryable = _context.Movies.AsQueryable();
            if(!string.IsNullOrEmpty(movieFilterDto.Title))
            {
                movieQueryable = movieQueryable.Where(m => m.Title.Contains(movieFilterDto.Title));
            }

            if (movieFilterDto.IsProjected)
            {
                movieQueryable = movieQueryable.Where(m => m.IsProyected);
            }

            if (movieFilterDto.NextPremiere)
            {
                movieQueryable = movieQueryable.Where(m => m.PremiereDate > DateTime.Today);
            }

            if (movieFilterDto.GenreId != 0)
            {
                movieQueryable = movieQueryable.Where(m =>
                                 m.Genres.Select(g => g.Id)
                                 .Contains(movieFilterDto.GenreId));
            }

            var movies = await movieQueryable.Include(m => m.Genres).ToListAsync();

            return Ok(_mapper.Map<List<MovieDto>>(movies));
        }



    }
}
