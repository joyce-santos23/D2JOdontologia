using Data.Patient;
using Data.Specialist;
using Data.Specialty;
using Domain.Entities;
using MedicalAppointmentSystem.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ClinicaDbContext : DbContext
    {
        public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }
        public virtual DbSet<Domain.Entities.Consultation> Consultation { get; set; }
        public virtual DbSet<Domain.Entities.Patient> Patient { get; set; }
        public virtual DbSet<Domain.Entities.Schedule> Schedule { get; set; }
        public virtual DbSet<Domain.Entities.Specialist> Specialist { get; set; }
        public virtual DbSet<Domain.Entities.Specialty> Specialty { get; set; }
        public virtual DbSet<Domain.Entities.User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Patient>()
            .HasBaseType<Domain.Entities.User>();
            modelBuilder.Entity<Domain.Entities.Specialist>()
            .HasBaseType<User>();

            modelBuilder.ApplyConfiguration(new ConsultationConfiguration());
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialistConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialtyConfiguration());
            
        }

    }
}
