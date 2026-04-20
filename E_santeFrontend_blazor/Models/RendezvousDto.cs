namespace E_santeFrontend.Models
{
    public class RendezvousDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int MedecinId { get; set; }
        public string? Date { get; set; }
        public string? Heure { get; set; }
        public string? Lieu { get; set; }
        public string? Statut { get; set; }
        public string? Description { get; set; }
    }
}
