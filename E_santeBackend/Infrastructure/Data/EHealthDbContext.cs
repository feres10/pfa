using Microsoft.EntityFrameworkCore;
using E_santeBackend.Domain.Entities;

namespace E_santeBackend.Infrastructure.Data
{
    public class EHealthDbContext : DbContext
    {
        public EHealthDbContext(DbContextOptions<EHealthDbContext> options) : base(options) { }

        public DbSet<CompteUtilisateur> Comptes { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Pharmacien> Pharmaciens { get; set; } = null!;
        public DbSet<Medecin> Medecins { get; set; } = null!;
        public DbSet<Pharmacie> Pharmacies { get; set; } = null!;
        public DbSet<Medicament> Medicaments { get; set; } = null!;
        public DbSet<StockMouvement> StockMouvements { get; set; } = null!;
        public DbSet<RendezVous> RendezVous { get; set; } = null!;
        public DbSet<Consultation> Consultations { get; set; } = null!;
        public DbSet<DossierMedical> DossiersMedicaux { get; set; } = null!;
        public DbSet<Ordonnance> Ordonnances { get; set; } = null!;
        public DbSet<LigneOrdonnance> LignesOrdonnances { get; set; } = null!;
        public DbSet<Paiement> Paiements { get; set; } = null!;
        public DbSet<Assurance> Assurances { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<CompteUtilisateur>()
                .HasOne(c => c.Role)
                .WithMany()
                .HasForeignKey(c => c.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.DossierMedical)
                .WithOne(d => d.Patient)
                .HasForeignKey<DossierMedical>(d => d.PatientId);

            // The database schema (existing migrations) does not include an Email column on Patients.
            // Ignore the `Email` property on the `Patient` entity so runtime model matches DB and
            // avoids insert/update errors when adding patients until migrations are updated.
            modelBuilder.Entity<Patient>().Ignore(p => p.Email);

            modelBuilder.Entity<Ordonnance>()
                .HasMany(o => o.Lignes)
                .WithOne()
                .HasForeignKey(l => l.OrdonnanceId);

            modelBuilder.Entity<LigneOrdonnance>()
                .HasOne(l => l.Medicament)
                .WithMany()
                .HasForeignKey(l => l.MedicamentId);
        }
    }
}