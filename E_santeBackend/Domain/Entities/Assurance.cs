namespace E_santeBackend.Domain.Entities
{
    public class Assurance
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int TauxCouverture { get; set; }
        public string NumeroCarte { get; set; } = string.Empty;
    }
}