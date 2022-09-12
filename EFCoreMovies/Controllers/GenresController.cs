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

        [HttpPost]
        public async Task<ActionResult> Post(Genre genre)
        {
            // _context.Entry(entidad).State => devuelve el estado de seguimiento de la entidad por dbContext
            var status1 = _context.Entry(genre).State; // Desasociado
            _context.Add(genre);
            var status2 = _context.Entry(genre).State; //Agregado
            await _context.SaveChangesAsync();
            var status3 = _context.Entry(genre).State; //Sin cambios
            return Ok();
        }

        [HttpPost("varios")]
        public async Task<ActionResult> Post(Genre[] genres)
        {
            // _context.AddRange => Permite agregar un conjunto de registros 
            _context.AddRange(genres);
            await _context.SaveChangesAsync();
            return Ok();

        }

        // Borrado normal
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = _context.Genres.FirstOrDefault(g => g.Id == id);
            if (genre is null)
            {
                return NotFound();
            }
            _context.Remove(genre);
            await _context.SaveChangesAsync();

            return Ok();

        }

        // Soft Delete => Sirve para marcar un registro como borrado pero no borrarlo de la tabla (permite no borrar data importante y poder hacer seguimiento, test, etc.)
        [HttpDelete("softDelete/{id:int}")]
        public async Task<ActionResult> softDelete(int id)
        {
            // Aplicamos eguimiento a la entidad recuperada
            var genre = await _context.Genres.AsTracking().FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
            {
                return NotFound();
            }

            // En este caso no borramos el registro de la bdd si no que seteamos la propiedad de borrado en true
            genre.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok();

        }

        // Endpoint para restaurar registros => en este caso registros borrados por soft delete
        [HttpPost("restore/{id:int}")]
        public async Task<ActionResult> Restore(int id)
        {
            // Aplicamos eguimiento a la entidad recuperada
            var genre = await _context.Genres.AsTracking()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(gen => gen.Id == id);

            if (genre is null)
            {
                return NotFound();
            }

            // En este caso no borramos el registro de la bdd si no que seteamos la propiedad de borrado en true
            genre.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}
