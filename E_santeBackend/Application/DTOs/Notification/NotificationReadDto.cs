namespace E_santeBackend.Application.DTOs.Notification
{
    public class NotificationReadDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateEnvoi { get; set; }
        public bool Lu { get; set; }
        public int CompteUtilisateurId { get; set; }
    }
}