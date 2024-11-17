using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Data.Patient
{
    public class PatientConfiguration : IEntityTypeConfiguration<Domain.Entities.Patient>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Patient> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Consultations)
                .WithOne(c => c.Patient)
                .HasForeignKey(c => c.PatientId);
        }
    }
}
