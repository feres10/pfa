using E_santeBackend.Application.DTOs.Ordonnance;
using E_santeBackend.Application.Interfaces;
using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Repositories;
using E_santeBackend.Shared.Constants;

namespace E_santeBackend.Application.Services
{
    public class OrdonnanceService : IOrdonnanceService
    {
        private readonly OrdonnanceRepository _repository;

        public OrdonnanceService(OrdonnanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrdonnanceReadDto>> GetAllAsync()
        {
            var ordonnances = await _repository.GetAllWithLignesAsync();
            return ordonnances.Select(o => new OrdonnanceReadDto
            {
                Id = o.Id,
                Date = o.Date,
                Statut = o.Statut,
                PatientId = o.PatientId,
                PharmacienId = o.PharmacienId,
                MedecinId = o.MedecinId,
                Lignes = o.Lignes?.Select(l => new LigneOrdonnanceReadDto { Id = l.Id, MedicamentId = l.MedicamentId, Quantite = l.Quantite, PrixUnitaire = l.PrixUnitaire }).ToList() ?? new List<LigneOrdonnanceReadDto>()
            }).ToList();
        }

        public async Task<OrdonnanceReadDto?> GetByIdAsync(int id)
        {
            var ordonnance = await _repository.GetWithLignesAsync(id);
            if (ordonnance == null) return null;

            return new OrdonnanceReadDto
            {
                Id = ordonnance.Id,
                Date = ordonnance.Date,
                Statut = ordonnance.Statut,
                PatientId = ordonnance.PatientId,
                PharmacienId = ordonnance.PharmacienId,
                MedecinId = ordonnance.MedecinId,
                Lignes = ordonnance.Lignes?.Select(l => new LigneOrdonnanceReadDto { Id = l.Id, MedicamentId = l.MedicamentId, Quantite = l.Quantite, PrixUnitaire = l.PrixUnitaire }).ToList() ?? new List<LigneOrdonnanceReadDto>()
            };
        }

        public async Task<OrdonnanceReadDto> CreateAsync(OrdonnanceCreateDto dto)
        {
            var ordonnance = new Ordonnance
            {
                Date = DateTime.UtcNow,
                Statut = AppConstants.StatutOrdonnance.EnAttente,
                PatientId = dto.PatientId,
                PharmacienId = dto.PharmacienId,
                MedecinId = dto.MedecinId,
                Lignes = dto.Lignes.Select(l => new LigneOrdonnance
                {
                    MedicamentId = l.MedicamentId,
                    Quantite = l.Quantite,
                    PrixUnitaire = l.PrixUnitaire
                }).ToList()
            };

            await _repository.AddAsync(ordonnance);

            return new OrdonnanceReadDto
            {
                Id = ordonnance.Id,
                Date = ordonnance.Date,
                Statut = ordonnance.Statut,
                PatientId = ordonnance.PatientId,
                PharmacienId = ordonnance.PharmacienId,
                MedecinId = ordonnance.MedecinId
            };
        }

        public async Task<bool> UpdateStatutAsync(int id, string statut)
        {
            var ordonnance = await _repository.GetByIdAsync(id);
            if (ordonnance == null) return false;

            ordonnance.Statut = statut;
            await _repository.UpdateAsync(ordonnance);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ordonnance = await _repository.GetByIdAsync(id);
            if (ordonnance == null) return false;

            await _repository.DeleteAsync(ordonnance);
            return true;
        }
    }
}