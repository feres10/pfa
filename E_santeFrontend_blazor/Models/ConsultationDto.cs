namespace E_santeFrontend.Models
{
    public class ConsultationDto
    {
        public int Id { get; set; }
        public string? Date { get; set; }
        public string? Heure { get; set; }
        public string? Diagnostic { get; set; }
        public int MedecinId { get; set; }
        public int PatientId { get; set; }
    }
}
