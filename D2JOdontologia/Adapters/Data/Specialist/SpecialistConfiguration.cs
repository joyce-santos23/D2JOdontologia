using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using System.Reflection.Emit;

namespace MedicalAppointmentSystem.Configurations
{
    public class SpecialistConfiguration : IEntityTypeConfiguration<Domain.Entities.Specialist>
    {
        public void Configure(EntityTypeBuilder<Specialist> builder)
        {
            builder.HasBaseType<Domain.Entities.User>();

            builder.HasOne(s => s.Specialty)
            .WithMany(sp => sp.Specialists)
            .HasForeignKey(s => s.SpecialtyId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
