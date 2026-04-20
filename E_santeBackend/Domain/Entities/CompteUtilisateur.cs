using System;

namespace E_santeBackend.Domain.Entities
{
    public class CompteUtilisateur
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;
        public bool Actif { get; set; }
        public int RoleId { get; set; }

        // Navigation
        public Role? Role { get; set; }

        public bool SeConnecter() => Actif;
        public void SeDeconnecter() { /* placeholder */ }
    }
}