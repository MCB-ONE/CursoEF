using EFCoreMovies.DataAccess;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Genre>> GetAll()
        {

            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genre>> GetById(int id)
        {
            //First() => Si no encuentra el registro que cumple la condición arroja EXCEPCIÓN
            //FirstOrDefault() => Si no encuentra el registro que cumple la condición devuelve NULL
            var genre = await _context.Genres.FirstOrDefaultAsync(genre => genre.Id == id);

            if (genre is null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        [HttpGet("filter")]
        public async Task<IEnumerable<Genre>> FilterByName(string name)
        {
            return await _context.Genres
                .Where(g => g.Name.Contains(name))
                .ToListAsync();
        }

        // Paginación de dos en dos elementos
        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetPagination(int page = 1)
        {
            var registresPerPage = 2;

            var genres = await _context.Genres
                .Skip((page - 1) * registresPerPage)
                .Take(registresPerPage)
                .ToListAsync();

            return Ok(genres);
        }



    }
}
