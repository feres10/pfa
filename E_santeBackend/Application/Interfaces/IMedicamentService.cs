using E_santeBackend.Application.DTOs.Medicament;

namespace E_santeBackend.Application.Interfaces
{
    public interface IMedicamentService
    {
        Task<List<MedicamentReadDto>> GetAllAsync();
        Task<MedicamentReadDto?> GetByIdAsync(int id);
        Task<List<MedicamentReadDto>> GetExpiringSoonAsync();
        Task<MedicamentReadDto> CreateAsync(E_santeBackend.Application.DTOs.Medicament.MedicamentCreateDto dto);
    }
}