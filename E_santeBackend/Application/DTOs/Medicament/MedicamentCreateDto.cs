using System;

namespace E_santeBackend.Application.DTOs.Medicament
{
    public class MedicamentCreateDto
    {
        public string Nom { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Fabricant { get; set; } = string.Empty;
        public int Stock { get; set; }
        public DateTime DateExpiration { get; set; }
    }
}
