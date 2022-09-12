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

        [HttpPost]
        public async Task<ActionResult> Post(ActorCreateDto actorCreateDto)
        {
            var actor = _mapper.Map<Actor>(actorCreateDto);
            _context.Add(actor);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Actualizar registro con modelo conectado (la misma instancia de DbContext realiza ambas operaciones, get y update) => Solo se genera un query con la actualización de los campos que han cambiado
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ActorCreateDto actorCreateDto, int id)
        {
            // Primero búscamos el actor en la bdd y configuramos seguimiento activo para poder realiza cambios con la mismma instancia de dbContext
            var actorDB = _context.Actors.AsTracking().FirstOrDefault(a => a.Id == id);

            if (actorDB == null)
            {
                return NotFound();
            }

            /* Mapeamos de actorDb a actorCreateDto 
             * si actorCreateDto viene de cliente con datos diferentes al regitro que habia en bdd las propiedades de la entidad son sobreescritas por mapper
             * actorDB = _mapper.Map(actorCreateDto, actorDB) => con esta línea de código hacemos que automapper mantenga la isma instancia de actorDB en memória
             *  => solo cambiamos sus propiedades, pero es l a misma instancia
             *  => de esta forma contextDb puede seguir haciendo el seguimiento y saber que su estatus pasa a modificado
             * */
            actorDB = _mapper.Map(actorCreateDto, actorDB);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Actualizar registro con modelo desconectado (un instandcia del dbContext se recupera el registro a actualizar y con otra instancia realizamos la actualización de la entidad) => Se realiza un query actualizando todos los campos (aúnque no hayan cambiado) MENOS EFICIENTE EN ESTE SENTIDO
        [HttpPut("disconnectedUpdate{id:int}")]
        public async Task<ActionResult> PutDisconnected(ActorCreateDto actorCreateDto, int id)
        {
            // AnyAsync => Es más rápido dado que solo devuleve true si existe la entidad en la bdd
            var actorExist = await _context.Actors.AnyAsync(a => a.Id == id);

            if (!actorExist)
            {
                return NotFound();
            }

            /*Marcamos el status de la entidad como modificada
             * => decimos que en la bdd hay un registro que representa al objeto con el que estamos trabajando y sus propiedadeshan sido modificadas
             * => por lo tanto al usar await _context.SaveChangesAsync() dicho registro será actualizado
             * */

            var actor = _mapper.Map<Actor>(actorCreateDto);
            actor.Id = id; //Seteamos el id del registro a actualizar
            _context.Update(actor);

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
