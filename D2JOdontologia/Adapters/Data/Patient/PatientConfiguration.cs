using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserEntity = Domain.Entities.User;
using PatientEntity = Domain.Entities.Patient;

namespace Data.Patient
{
    public class PatientConfiguration : IEntityTypeConfiguration<PatientEntity>
    {
        public void Configure(EntityTypeBuilder<PatientEntity> builder)
        {
            builder.HasBaseType<UserEntity>();

            builder.HasMany(p => p.Consultations)
                .WithOne(c => c.Patient)
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Cascade); ;

        }
    }
}
