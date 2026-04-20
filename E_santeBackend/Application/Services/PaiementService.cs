using E_santeBackend.Application.DTOs.Paiement;
using E_santeBackend.Application.Interfaces;
using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Repositories;
using E_santeBackend.Shared.Constants;

namespace E_santeBackend.Application.Services
{
    public class PaiementService : IPaiementService
    {
        private readonly PaiementRepository _repository;

        public PaiementService(PaiementRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaiementReadDto> CreateAsync(PaiementCreateDto dto)
        {
            var paiement = new Paiement
            {
                Montant = dto.Montant,
                DatePaiement = DateTime.UtcNow,
                Methode = dto.Methode,
                Statut = AppConstants.StatutPaiement.Paye,
                OrdonnanceId = dto.OrdonnanceId,
                PatientId = dto.PatientId
            };

            await _repository.AddAsync(paiement);

            return new PaiementReadDto
            {
                Id = paiement.Id,
                Montant = paiement.Montant,
                DatePaiement = paiement.DatePaiement,
                Methode = paiement.Methode,
                Statut = paiement.Statut,
                OrdonnanceId = paiement.OrdonnanceId,
                PatientId = paiement.PatientId
            };
        }

        public async Task<List<PaiementReadDto>> GetByPatientIdAsync(int patientId)
        {
            var paiements = await _repository.GetByPatientIdAsync(patientId);
            return paiements.Select(p => new PaiementReadDto
            {
                Id = p.Id,
                Montant = p.Montant,
                DatePaiement = p.DatePaiement,
                Methode = p.Methode,
                Statut = p.Statut,
                OrdonnanceId = p.OrdonnanceId,
                PatientId = p.PatientId
            }).ToList();
        }
    }
}