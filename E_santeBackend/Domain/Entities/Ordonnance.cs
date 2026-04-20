using System;
using System.Collections.Generic;

namespace E_santeBackend.Domain.Entities
{
    public class Ordonnance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Statut { get; set; } = string.Empty;

        public int PatientId { get; set; }
        public int PharmacienId { get; set; }
        public int MedecinId { get; set; }

        // Navigation
        public List<LigneOrdonnance> Lignes { get; set; } = new();
    }
}