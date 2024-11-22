using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecialtyEntity = Domain.Entities.Specialty;
using SpecialistEntity = Domain.Entities.Specialist;
using UserEntity = Domain.Entities.User;

namespace Data.Specialist
{
    public class SpecialistConfiguration : IEntityTypeConfiguration<SpecialistEntity>
    {
        public void Configure(EntityTypeBuilder<SpecialistEntity> builder)
        {
            builder.HasBaseType<UserEntity>();

            builder.HasMany(s => s.Specialties)
                .WithMany(sp => sp.Specialists)
                .UsingEntity<Dictionary<string, object>>(
                    "SpecialistSpecialty", // Nome da tabela de junção
                    j => j.HasOne<SpecialtyEntity>().WithMany().HasForeignKey("SpecialtyId").OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<SpecialistEntity>().WithMany().HasForeignKey("SpecialistId").OnDelete(DeleteBehavior.NoAction)
                );
        }
    }
}
