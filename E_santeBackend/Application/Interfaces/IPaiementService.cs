using E_santeBackend.Application.DTOs.Paiement;

namespace E_santeBackend.Application.Interfaces
{
    public interface IPaiementService
    {
        Task<PaiementReadDto> CreateAsync(PaiementCreateDto dto);
        Task<List<PaiementReadDto>> GetByPatientIdAsync(int patientId);
    }

    public class PaiementReadDto
    {
        public int Id { get; set; }
        public double Montant { get; set; }
        public DateTime DatePaiement { get; set; }
        public string Methode { get; set; } = string.Empty;
        public string Statut { get; set; } = string.Empty;
        public int OrdonnanceId { get; set; }
        public int PatientId { get; set; }
    }
}