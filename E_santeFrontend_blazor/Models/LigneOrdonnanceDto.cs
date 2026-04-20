namespace E_santeFrontend.Models
{
    public class LigneOrdonnanceDto
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}
