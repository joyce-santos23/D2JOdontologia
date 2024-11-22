using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecialtyEntity = Domain.Entities.Specialty;
using SpecialistEntity = Domain.Entities.Specialist;



namespace Data.Specialty
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<SpecialtyEntity>
    {
        public void Configure(EntityTypeBuilder<SpecialtyEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(sp => sp.Specialists)
                .WithMany(s => s.Specialties)
                .UsingEntity<Dictionary<string, object>>(
                    "SpecialistSpecialty", // Nome da tabela de junção
                    j => j.HasOne<SpecialistEntity>().WithMany().HasForeignKey("SpecialistId").OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<SpecialtyEntity>().WithMany().HasForeignKey("SpecialtyId").OnDelete(DeleteBehavior.NoAction)
                );

            builder.HasData(
                new SpecialtyEntity { Id = -1, Name = "Ortodontia" },
                new SpecialtyEntity { Id = -2, Name = "Periodontia" },
                new SpecialtyEntity { Id = -3, Name = "Endodontia" },
                new SpecialtyEntity { Id = -4, Name = "Prótese Dentária" },
                new SpecialtyEntity { Id = -5, Name = "Cirurgia Oral" },
                new SpecialtyEntity { Id = -6, Name = "Odontopediatria" },
                new SpecialtyEntity { Id = -7, Name = "Implantodontia" },
                new SpecialtyEntity { Id = -8, Name = "Odontologia Estética" }
            );




        }
    }
}
