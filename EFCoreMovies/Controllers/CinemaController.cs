using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DataAccess;
using EFCoreMovies.DTOs.Cinema;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFCoreMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CinemaController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CinemaDto>> Get()
        {
            /* Necesitamos serializar el typo de dato point (geolocalización) 
             *  => Hemos de mappearlo para extraer la data de location y setear longitud y latitud en el modelDto
             */
            return await _context.Cinemas.ProjectTo<CinemaDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        /*
         * Ruta par adevolver los cines más cercanos a unas cordenadas concretas
         *  -> Simulamos unas coordenadas de entrada con la librería NetTopologySuite.Geometries
         *  
         
         */
        [HttpGet("nearest")]
        public async Task<ActionResult> Get(double latitude, double longitude)
        {
            //Instancia de libreria para poder hacer mediciones en el planeta tierra
            var geaometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            // Generamos un punto geográfico
            var myLocation = geaometryFactory.CreatePoint(new Coordinate(longitude, latitude));

            // Bonus: Podemos filtrar los registros que estén mas lejanos de x metros
            // Y usando la clausula where y el método de IsWithinDistance() que proporciona Location(poin type)
            var maxDistance = 5000;

            // Ordenamos los resultados según la distancia de los regsitros de cine al punto generado
            var cinemas = await _context.Cinemas
                .OrderBy(c => c.Location.Distance(myLocation))
                .Where(c => c.Location.IsWithinDistance(myLocation, maxDistance))
                .Select(c => new
                {
                    Name = c.Name,
                    Distance = Math.Round(c.Location.Distance(myLocation))
                })
                .ToListAsync();

            return Ok(cinemas);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CinemaCreateDto cinemaCreateDto)
        {
            var cinema = _mapper.Map<Cinema>(cinemaCreateDto);
            _context.Add(cinema);   
            await _context.SaveChangesAsync();
            return Ok();
        }



    }
}
