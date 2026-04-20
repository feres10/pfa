namespace E_santeBackend.Domain.Entities
{
    public class Pharmacie
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
    }
}