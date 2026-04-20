using System;

namespace E_santeBackend.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateEnvoi { get; set; }
        public bool Lu { get; set; }

        public int CompteUtilisateurId { get; set; }

        // Navigation
        public CompteUtilisateur? CompteUtilisateur { get; set; }
    }
}