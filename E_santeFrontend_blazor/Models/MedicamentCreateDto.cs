namespace E_santeFrontend.Models
{
    public class MedicamentCreateDto
    {
        public string? Nom { get; set; }
        public string? Dosage { get; set; }
        public string? Fabricant { get; set; }
        public int Stock { get; set; }
        public System.DateTime? DateExpiration { get; set; }
    }
}
