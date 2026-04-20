using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>
    {
        public PatientRepository(EHealthDbContext context) : base(context) { }

        public async Task<Patient?> GetByCompteIdAsync(int compteId)
        {
            return await _dbSet.Include(p => p.DossierMedical).FirstOrDefaultAsync(p => p.Id == compteId);
        }

        // Fallback loader that selects only columns known to exist in the DB schema.
        // This is used when the database schema hasn't been migrated to include the `Email` column yet.
        public async Task<List<Patient>> GetAllWithoutEmailAsync()
        {
            // Use a raw ADO.NET query so EF doesn't try to map missing columns (like shadow FKs).
            var list = new List<Patient>();
            var conn = _context.Database.GetDbConnection();
            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT \"Id\", \"Nom\", \"Prenom\", \"DateNaissance\", \"Cin\", \"Telephone\" FROM \"Patients\"";
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var p = new Patient
                    {
                        Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        Nom = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Prenom = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        DateNaissance = reader.IsDBNull(3) ? default : reader.GetFieldValue<DateTime>(3),
                        Cin = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        Telephone = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                    };
                    list.Add(p);
                }
            }
            finally
            {
                try { await conn.CloseAsync(); } catch { }
            }

            return list;
        }

        // Override GetByIdAsync to provide a safe fallback when the DB schema
        // does not contain newly-added columns (like Email). We try the normal
        // EF path first and on failure use a simple ADO.NET projection.
        public override async Task<Patient?> GetByIdAsync(int id)
        {
            try
            {
                return await base.GetByIdAsync(id);
            }
            catch (System.Exception)
            {
                var conn = _context.Database.GetDbConnection();
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        await conn.OpenAsync();

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT \"Id\", \"Nom\", \"Prenom\", \"DateNaissance\", \"Cin\", \"Telephone\" FROM \"Patients\" WHERE \"Id\" = @id";
                    var pparam = cmd.CreateParameter();
                    pparam.ParameterName = "@id";
                    pparam.Value = id;
                    cmd.Parameters.Add(pparam);

                    using var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        return new Patient
                        {
                            Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            Nom = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Prenom = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            DateNaissance = reader.IsDBNull(3) ? default : reader.GetFieldValue<System.DateTime>(3),
                            Cin = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            Telephone = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        };
                    }
                }
                finally
                {
                    try { await conn.CloseAsync(); } catch { }
                }

                return null;
            }
        }
    }
}