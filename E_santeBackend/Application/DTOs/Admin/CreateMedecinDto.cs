namespace E_santeBackend.Application.DTOs.Admin
{
    public class CreateMedecinDto
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Specialite { get; set; }
        public string? CodeMedecin { get; set; }
    }
}
