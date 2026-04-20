using System;

namespace E_santeBackend.Domain.Entities
{
    public class StockMouvement
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public DateTime Date { get; set; }
        public int MedicamentId { get; set; }

        // Navigation
        public Medicament? Medicament { get; set; }
    }
}