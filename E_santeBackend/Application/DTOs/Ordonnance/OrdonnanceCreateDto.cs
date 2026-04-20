namespace E_santeBackend.Application.DTOs.Ordonnance
{
    public class OrdonnanceCreateDto
    {
        public int PatientId { get; set; }
        public int PharmacienId { get; set; }
        public int MedecinId { get; set; }
        public List<LigneOrdonnanceDto> Lignes { get; set; } = new();
    }

    public class LigneOrdonnanceDto
    {
        public int MedicamentId { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}