using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ClinicaDbContext : DbContext
    {
        public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }
        public virtual DbSet<Domain.Entities.Consultation> Consultation { get; set; }
        public virtual DbSet<Domain.Entities.Patient> Patient { get; set; }
        public virtual DbSet<Domain.Entities.Procedure> Procedure { get; set; }
        public virtual DbSet<Domain.Entities.Schedule> Schedule { get; set; }
        public virtual DbSet<Domain.Entities.Specialist> Specialist { get; set; }
        public virtual DbSet<Domain.Entities.Specialty> Specialty { get; set; }
        public virtual DbSet<Domain.Entities.User> User { get; set; }

    }
}
