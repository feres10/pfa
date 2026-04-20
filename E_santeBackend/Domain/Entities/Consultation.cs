using System;

namespace E_santeBackend.Domain.Entities
{
    public class Consultation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Diagnostic { get; set; } = string.Empty;
        public int MedecinId { get; set; }
        public int PatientId { get; set; }
    }
}