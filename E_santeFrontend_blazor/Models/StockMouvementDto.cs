using System;

namespace E_santeFrontend.Models
{
    public class StockMouvementDto
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; }
        public int Quantite { get; set; }
        public DateTime Date { get; set; }
        public string? Type { get; set; }
    }
}
