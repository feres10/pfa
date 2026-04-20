using E_santeBackend.Application.DTOs.Patient;
using E_santeBackend.Application.Interfaces;
using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace E_santeBackend.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly PatientRepository _repository;

        public PatientService(PatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PatientReadDto>> GetAllAsync()
        {
            try
            {
                var patients = await _repository.GetAllAsync();
                return patients.Select(p => new PatientReadDto
                {
                    Id = p.Id,
                    Nom = p.Nom,
                    Prenom = p.Prenom,
                    Email = p is not null ? (p.Email ?? string.Empty) : string.Empty,
                    DateNaissance = p.DateNaissance,
                    Cin = p.Cin,
                    Telephone = p.Telephone
                }).ToList();
            }
            catch (System.Exception)
            {
                // If the DB schema is not yet migrated (missing Email column), use a fallback query
                // that selects only existing columns and returns DTOs with an empty Email.
                var patients = await _repository.GetAllWithoutEmailAsync();
                return patients.Select(p => new PatientReadDto
                {
                    Id = p.Id,
                    Nom = p.Nom,
                    Prenom = p.Prenom,
                    Email = string.Empty,
                    DateNaissance = p.DateNaissance,
                    Cin = p.Cin,
                    Telephone = p.Telephone
                }).ToList();
            }
        }

        public async Task<PatientReadDto?> GetByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return null;

            return new PatientReadDto
            {
                Id = patient.Id,
                Nom = patient.Nom,
                Prenom = patient.Prenom,
                Email = patient.Email ?? string.Empty,
                DateNaissance = patient.DateNaissance,
                Cin = patient.Cin,
                Telephone = patient.Telephone
            };
        }

        public async Task<PatientReadDto> CreateAsync(PatientCreateDto dto)
        {
            var patient = new Patient
            {
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Email = dto.Email,
                DateNaissance = dto.DateNaissance,
                Cin = dto.Cin,
                Telephone = dto.Telephone
            };

            await _repository.AddAsync(patient);

            return new PatientReadDto
            {
                Id = patient.Id,
                Nom = patient.Nom,
                Prenom = patient.Prenom,
                Email = patient.Email ?? string.Empty,
                DateNaissance = patient.DateNaissance,
                Cin = patient.Cin,
                Telephone = patient.Telephone
            };
        }

        public async Task<bool> UpdateAsync(int id, PatientUpdateDto dto)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return false;

            if (!string.IsNullOrEmpty(dto.Nom)) patient.Nom = dto.Nom;
            if (!string.IsNullOrEmpty(dto.Prenom)) patient.Prenom = dto.Prenom;
            if (!string.IsNullOrEmpty(dto.Email)) patient.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Telephone)) patient.Telephone = dto.Telephone;

            await _repository.UpdateAsync(patient);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return false;

            await _repository.DeleteAsync(patient);
            return true;
        }
    }
}