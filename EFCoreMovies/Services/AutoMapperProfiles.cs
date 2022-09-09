using AutoMapper;
using EFCoreMovies.DTOs.Actor;
using EFCoreMovies.DTOs.Cinema;
using EFCoreMovies.DTOs.Genre;
using EFCoreMovies.DTOs.MovieDto;
using EFCoreMovies.Entities;

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

        }


    }
}
