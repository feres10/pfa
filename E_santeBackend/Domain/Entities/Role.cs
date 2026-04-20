namespace E_santeBackend.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}