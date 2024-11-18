using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace MedicalAppointmentSystem.Configurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Data)
                   .IsRequired();

            builder.HasOne(s => s.Specialty)
                   .WithMany(sp => sp.Schedules)
                   .HasForeignKey(s => s.SpecialtyId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s => s.Consultations)
                   .WithOne(c => c.Schedule)
                   .HasForeignKey(c => c.ScheduleId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
