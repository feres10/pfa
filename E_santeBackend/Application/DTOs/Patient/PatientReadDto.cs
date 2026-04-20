namespace E_santeBackend.Application.DTOs.Patient
{
    public class PatientReadDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateNaissance { get; set; }
        public string Cin { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
    }
}