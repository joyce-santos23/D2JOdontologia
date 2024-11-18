using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Data.Patient
{
    public class PatientConfiguration : IEntityTypeConfiguration<Domain.Entities.Patient>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Patient> builder)
        {
            builder.HasBaseType<Domain.Entities.User>();

            builder.HasMany(p => p.Consultations)
                .WithOne(c => c.Patient)
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Cascade); ;

        }
    }
}
