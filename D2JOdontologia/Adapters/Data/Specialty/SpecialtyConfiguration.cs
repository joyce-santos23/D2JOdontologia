using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace MedicalAppointmentSystem.Configurations
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Domain.Entities.Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasKey(s => s.Id);
        }

    }
}
