
namespace E_santeBackend.Domain.Entities
{
    public class Medecin
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Specialite { get; set; } = string.Empty;
        public string CodeMedecin { get; set; } = string.Empty;

        // Navigation
        public CompteUtilisateur? CompteUtilisateur { get; set; }
    }
}