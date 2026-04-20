namespace E_santeFrontend.Models
{
    public class RegisterDto
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; } = "Patient";
        public string? Telephone { get; set; }
    }
}
