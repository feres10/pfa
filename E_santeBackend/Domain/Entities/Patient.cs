using System;

namespace E_santeBackend.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateNaissance { get; set; }
        public string Cin { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;

        // Navigation
        public CompteUtilisateur? CompteUtilisateur { get; set; }
        public DossierMedical? DossierMedical { get; set; }
    }
}