namespace E_santeBackend.Application.DTOs.Ordonnance
{
    public class LigneOrdonnanceReadDto
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}
