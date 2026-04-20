using E_santeBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(EHealthDbContext context)
        {
            // Ensure database is created
            await context.Database.MigrateAsync();

            // Check if roles already exist
            if (await context.Roles.AnyAsync())
            {
                return; // DB has been seeded
            }

            // Seed Roles
            var roles = new List<Role>
            {
                new Role { Nom = "Admin", Description = "Administrateur systéme" },
                new Role { Nom = "Medecin", Description = "Médecin praticien" },
                new Role { Nom = "Pharmacien", Description = "Pharmacien" },
                new Role { Nom = "Patient", Description = "Patient" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

            // Optional: Seed a default pharmacy
            var pharmacie = new Pharmacie
            {
                Nom = "Pharmacie Centrale",
                Adresse = "123 Rue Principal",
                Ville = "Tunis",
                Telephone = "71234567"
            };

            await context.Pharmacies.AddAsync(pharmacie);
            await context.SaveChangesAsync();

            // Seed default admin account for local testing (only if no admin exists)
            // Only create this default admin in Development or when explicitly allowed via SEED_ADMIN_ALLOWED env var.
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var allowSeedAdmin = string.Equals(env, "Development", System.StringComparison.OrdinalIgnoreCase)
                                 || string.Equals(System.Environment.GetEnvironmentVariable("SEED_ADMIN_ALLOWED"), "true", System.StringComparison.OrdinalIgnoreCase);

            if (allowSeedAdmin && !await context.Comptes.AnyAsync(c => c.Role != null && c.Role.Nom == "Admin"))
            {
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Nom == "Admin");
                if (adminRole != null)
                {
                    var defaultPassword = System.Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD") ?? "Admin123!";
                    var adminCompte = new CompteUtilisateur
                    {
                        Email = "admin@localhost",
                        MotDePasse = E_santeBackend.Shared.Helpers.PasswordHasher.HashPassword(defaultPassword),
                        Actif = true,
                        RoleId = adminRole.Id
                    };
                    await context.Comptes.AddAsync(adminCompte);
                    await context.SaveChangesAsync();

                    try
                    {
                        System.Console.WriteLine($"[DbInitializer] Seeded default admin 'admin@localhost' (env={env}).");
                    }
                    catch { }
                }
            }
        }
    }
}