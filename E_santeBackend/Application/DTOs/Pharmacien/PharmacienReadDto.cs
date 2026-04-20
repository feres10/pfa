namespace E_santeBackend.Application.DTOs.Pharmacien
{
    public class PharmacienReadDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Licence { get; set; } = string.Empty;
        public int PharmacieId { get; set; }
    }
}