using E_santeBackend.Application.DTOs.Ordonnance;

namespace E_santeBackend.Application.Interfaces
{
    public interface IOrdonnanceService
    {
        Task<List<OrdonnanceReadDto>> GetAllAsync();
        Task<OrdonnanceReadDto?> GetByIdAsync(int id);
        Task<OrdonnanceReadDto> CreateAsync(OrdonnanceCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatutAsync(int id, string statut);
    }
}