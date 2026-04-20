using System;

namespace E_santeBackend.Domain.Entities
{
    public class RendezVous
    {
        public int Id { get; set; }
        public DateTime DateRDV { get; set; }
        public string Statut { get; set; } = string.Empty;

        // Navigation
        public int PatientId { get; set; }
        public int MedecinId { get; set; }
    }
}