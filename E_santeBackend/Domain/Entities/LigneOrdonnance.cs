namespace E_santeBackend.Domain.Entities
{
    public class LigneOrdonnance
    {
        public int Id { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }

        public int MedicamentId { get; set; }
        public int OrdonnanceId { get; set; }

        // Navigation
        public Medicament? Medicament { get; set; }
    }
}