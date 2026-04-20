using System;

namespace E_santeBackend.Domain.Entities
{
    public class Paiement
    {
        public int Id { get; set; }
        public double Montant { get; set; }
        public DateTime DatePaiement { get; set; }
        public string Methode { get; set; } = string.Empty;
        public string Statut { get; set; } = string.Empty;

        public int OrdonnanceId { get; set; }
        public int PatientId { get; set; }
    }
}