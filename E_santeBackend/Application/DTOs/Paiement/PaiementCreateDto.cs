namespace E_santeBackend.Application.DTOs.Paiement
{
    public class PaiementCreateDto
    {
        public double Montant { get; set; }
        public string Methode { get; set; } = string.Empty;
        public int OrdonnanceId { get; set; }
        public int PatientId { get; set; }
    }
}