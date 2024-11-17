using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Data.Patient
{
    public class ConsultationConfiguration : IEntityTypeConfiguration<Domain.Entities.Consultation>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Consultation> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(c => c.Patient)
                   .WithMany(p => p.Consultations)
                   .HasForeignKey(c => c.PatientId);

            builder.HasOne(c => c.Procedure)
                   .WithMany(p => p.Consultations)
                   .HasForeignKey(c => c.ProcedureId);

            builder.HasOne(c => c.Schedule)
                   .WithMany(s => s.Consultations)
                   .HasForeignKey(c => c.ScheduleId);
        }
    }
}
