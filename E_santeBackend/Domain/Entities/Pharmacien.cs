namespace E_santeBackend.Domain.Entities
{
    public class Pharmacien
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Licence { get; set; } = string.Empty;
        public int PharmacieId { get; set; }

        // Navigation
        public CompteUtilisateur? CompteUtilisateur { get; set; }
        public Pharmacie? Pharmacie { get; set; }
    }
}