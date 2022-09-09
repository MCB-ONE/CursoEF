using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMovies.Entities.Config
{
    public class CinemaRoomConfig: IEntityTypeConfiguration<CinemaRoom>
    {
        public void Configure(EntityTypeBuilder<CinemaRoom> builder)
        {
            // Usamos api fluent para setear un valor por defecto en una columna
            builder.Property(prop => prop.RoomType)
                .HasDefaultValue(RoomType.TwoDimensions);
        }

    }
}
