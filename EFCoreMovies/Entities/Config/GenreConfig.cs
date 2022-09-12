using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMovies.Entities.Config
{
    public class GenreConfig : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property(prop => prop.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Aplicamos filtro a nivel de modelo => En este caso para soft delete
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
