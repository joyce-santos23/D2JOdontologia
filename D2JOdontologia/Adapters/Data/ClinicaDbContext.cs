using Data.Patient;
using Data.Specialist;
using Data.Specialty;
using UserEntity = Domain.Entities.User;
using PatientEntity = Domain.Entities.Patient;
using SpecialistEntity = Domain.Entities.Specialist;
using MedicalAppointmentSystem.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ClinicaDbContext : DbContext
    {
        public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }
        public virtual DbSet<Domain.Entities.Consultation> Consultation { get; set; }
        public virtual DbSet<PatientEntity> Patient { get; set; }
        public virtual DbSet<Domain.Entities.Schedule> Schedule { get; set; }
        public virtual DbSet<SpecialistEntity> Specialist { get; set; }
        public virtual DbSet<Domain.Entities.Specialty> Specialty { get; set; }
        public virtual DbSet<UserEntity> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientEntity>()
            .HasBaseType<UserEntity>();
            modelBuilder.Entity<SpecialistEntity>()
            .HasBaseType<UserEntity>();
            modelBuilder.Entity<UserEntity>()
        .HasDiscriminator<string>("Role") // Nome da coluna no banco
        .HasValue<PatientEntity>("Patient")     // Define "Patient" para a entidade Patient
        .HasValue<SpecialistEntity>("Specialist");

            modelBuilder.ApplyConfiguration(new ConsultationConfiguration());
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialistConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialtyConfiguration());
            
        }

    }
}
