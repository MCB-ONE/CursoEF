using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMovies.Entities.Config
{
    public class ActorConfig : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(prop => prop.DateOfBirth)
                .HasColumnType("date");

            builder.Property(prop => prop.Name)
               .HasMaxLength(150)
               .IsRequired();

            // Configuración de mapeo flexible. 
            builder.Property(prop => prop.Name).HasField("_name");
        }
    }
}
