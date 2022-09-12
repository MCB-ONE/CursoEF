using AutoMapper;
using EFCoreMovies.DTOs.Actor;
using EFCoreMovies.DTOs.Cinema;
using EFCoreMovies.DTOs.Genre;
using EFCoreMovies.DTOs.MovieDto;
using EFCoreMovies.Entities;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFCoreMovies.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Actor, ActorDto>();

            CreateMap<Cinema, CinemaDto>()
                .ForMember(dto => dto.Latitude, ent => ent.MapFrom(prop => prop.Location.Y))
                .ForMember(dto => dto.Longitude, ent => ent.MapFrom(prop => prop.Location.X));

            CreateMap<Genre, GenreDto>();

            CreateMap<Movie, MovieDto>()
                .ForMember(dto => dto.Cinemas,
                ent => ent.MapFrom(prop => prop.CinemaRooms.Select(room => room.Cinema)))
                .ForMember(dto => dto.Actors,
                ent => ent.MapFrom(prop => prop.MoviesActors.Select(movActor => movActor.Actor)));


            // Configuración Dtos para la creación de cinema y sus entidades relacionadas no existentes
            var geaometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            CreateMap<CinemaCreateDto, Cinema>()
                .ForMember(ent => ent.Location, 
                dto => dto.MapFrom(prop => geaometryFactory
                .CreatePoint(new Coordinate(prop.Longitude, prop.Latitude))));

            CreateMap<CinemaDiscountCreateDto, CinemaDiscount>();
            CreateMap<CinemaRoomCreateDto, CinemaRoom>();

            // Configuración Dtos para la creación de pelicuas y entidades relacionadas ya existentes
            CreateMap<MovieCreateDto, Movie>()
                 .ForMember(ent => ent.Genres,
                 dto => dto.MapFrom(prop => prop.Genres
                 .Select(id => new Genre() { Id = id })))
            .ForMember(ent => ent.CinemaRooms,
                 dto => dto.MapFrom(prop => prop.CinemaRooms
                 .Select(id => new CinemaRoom() { Id = id })));
            CreateMap<MovieActorCreateDto, MovieActor>();

            // Configuración dto para creación aislada de actor
            CreateMap<ActorCreateDto, Actor>();
        }


    }
}
