namespace E_santeBackend.Application.DTOs.Patient
{
    public class PatientUpdateDto
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
    }
}