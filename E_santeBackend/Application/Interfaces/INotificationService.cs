using E_santeBackend.Application.DTOs.Notification;

namespace E_santeBackend.Application.Interfaces
{
    public interface INotificationService
    {
        Task<bool> CreateAsync(int compteId, string message);
        Task<List<NotificationReadDto>> GetByCompteIdAsync(int compteId);
        Task<bool> MarquerCommeLueAsync(int id);
    }
}