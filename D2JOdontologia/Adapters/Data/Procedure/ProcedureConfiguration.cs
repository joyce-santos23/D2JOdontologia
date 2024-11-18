using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Data.Procedure
{
    public class ProcedureConfiguration : IEntityTypeConfiguration<Domain.Entities.Procedure>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Procedure> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Consultations)
                   .WithOne(c => c.Procedure)
                   .HasForeignKey(c => c.ProcedureId)
                   .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
