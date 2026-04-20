namespace E_santeBackend.Application.DTOs.Medicament
{
    public class MedicamentReadDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Fabricant { get; set; } = string.Empty;
        public int Stock { get; set; }
        public DateTime DateExpiration { get; set; }
    }
}