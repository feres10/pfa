namespace E_santeBackend.Application.DTOs.Ordonnance
{
    public class OrdonnanceReadDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Statut { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public int PharmacienId { get; set; }
        public int MedecinId { get; set; }
        public List<LigneOrdonnanceReadDto> Lignes { get; set; } = new();
    }
}