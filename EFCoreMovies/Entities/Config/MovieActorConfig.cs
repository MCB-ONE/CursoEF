using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMovies.Entities.Config
{
    public class MovieActorConfig: IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            builder
                .Property(prop => prop.Character)
                .HasMaxLength(150);

            builder
                .HasKey(prop => new { prop.MovieId, prop.ActorId });
        }

    }
}
