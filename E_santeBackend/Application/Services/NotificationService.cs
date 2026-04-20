using E_santeBackend.Application.DTOs.Notification;
using E_santeBackend.Application.Interfaces;
using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Repositories;

namespace E_santeBackend.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepository _repository;

        public NotificationService(NotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(int compteId, string message)
        {
            var notification = new Notification
            {
                Message = message,
                DateEnvoi = DateTime.UtcNow,
                Lu = false,
                CompteUtilisateurId = compteId
            };

            await _repository.AddAsync(notification);
            return true;
        }

        public async Task<List<NotificationReadDto>> GetByCompteIdAsync(int compteId)
        {
            var notifications = await _repository.GetByCompteIdAsync(compteId);
            return notifications.Select(n => new NotificationReadDto
            {
                Id = n.Id,
                Message = n.Message,
                DateEnvoi = n.DateEnvoi,
                Lu = n.Lu,
                CompteUtilisateurId = n.CompteUtilisateurId
            }).ToList();
        }

        public async Task<bool> MarquerCommeLueAsync(int id)
        {
            var notification = await _repository.GetByIdAsync(id);
            if (notification == null) return false;

            notification.Lu = true;
            await _repository.UpdateAsync(notification);
            return true;
        }
    }
}