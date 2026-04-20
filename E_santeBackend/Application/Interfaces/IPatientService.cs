using E_santeBackend.Application.DTOs.Patient;

namespace E_santeBackend.Application.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientReadDto>> GetAllAsync();
        Task<PatientReadDto?> GetByIdAsync(int id);
        Task<PatientReadDto> CreateAsync(PatientCreateDto dto);
        Task<bool> UpdateAsync(int id, PatientUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}