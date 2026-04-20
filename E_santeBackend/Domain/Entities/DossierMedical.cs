namespace E_santeBackend.Domain.Entities
{
    public class DossierMedical
    {
        public int Id { get; set; }
        public string GroupeSanguin { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public string MaladiesChroniques { get; set; } = string.Empty;
        public int PatientId { get; set; }

        // Navigation
        public Patient? Patient { get; set; }
    }
}