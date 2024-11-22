using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Patient
{
    public class ConsultationConfiguration : IEntityTypeConfiguration<Domain.Entities.Consultation>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Consultation> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Patient)
                   .WithMany(p => p.Consultations)
                   .HasForeignKey(c => c.PatientId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Patient)
                   .WithMany(p => p.Consultations)
                   .HasForeignKey(c => c.PatientId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Schedule)
                   .WithMany(s => s.Consultations)
                   .HasForeignKey(c => c.ScheduleId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
