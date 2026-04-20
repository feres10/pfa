namespace E_santeBackend.Application.DTOs.Medecin
{
    public class MedecinReadDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialite { get; set; } = string.Empty;
        public string CodeMedecin { get; set; } = string.Empty;
    }
}