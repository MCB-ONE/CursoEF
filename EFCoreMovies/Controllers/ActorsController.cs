using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DataAccess;
using EFCoreMovies.DTOs.Actor;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            //Seleccionar campos directamnete con select queries
            //var actors = await _context.Actors.Select(a => new { a.Id, a.Name }).ToListAsync();

            //Seleccionar campos usando automapper
            var actors = await _context.Actors
                .ProjectTo<ActorDto>(_mapper.ConfigurationProvider).ToListAsync();

            return Ok(actors);
        }
    }
}
