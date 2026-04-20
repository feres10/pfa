namespace E_santeFrontend.Models
{
    public class AdminCreatePharmacienDto
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Telephone { get; set; }
        public string? Licence { get; set; }
        public int PharmacieId { get; set; }
    }
}
