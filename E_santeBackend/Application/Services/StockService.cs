using E_santeBackend.Application.DTOs.Stock;
using E_santeBackend.Application.Interfaces;
using E_santeBackend.Domain.Entities;
using E_santeBackend.Infrastructure.Repositories;

namespace E_santeBackend.Application.Services
{
    public class StockService : IStockService
    {
        private readonly StockRepository _stockRepository;
        private readonly MedicamentRepository _medicamentRepository;

        public StockService(StockRepository stockRepository, MedicamentRepository medicamentRepository)
        {
            _stockRepository = stockRepository;
            _medicamentRepository = medicamentRepository;
        }

        public async Task<bool> AjouterMouvementAsync(StockMouvementCreateDto dto)
        {
            var medicament = await _medicamentRepository.GetByIdAsync(dto.MedicamentId);
            if (medicament == null) return false;

            var mouvement = new StockMouvement
            {
                Type = dto.Type,
                Quantite = dto.Quantite,
                Date = DateTime.UtcNow,
                MedicamentId = dto.MedicamentId
            };

            if (dto.Type == "Entree")
            {
                medicament.Stock += dto.Quantite;
            }
            else if (dto.Type == "Sortie")
            {
                if (medicament.Stock < dto.Quantite)
                    return false;
                medicament.Stock -= dto.Quantite;
            }

            await _stockRepository.AddAsync(mouvement);
            await _medicamentRepository.UpdateAsync(medicament);

            return true;
        }

        public async Task<List<StockMouvementReadDto>> GetByMedicamentIdAsync(int medicamentId)
        {
            var mouvements = await _stockRepository.GetByMedicamentIdAsync(medicamentId);
            return mouvements.Select(m => new StockMouvementReadDto
            {
                Id = m.Id,
                Type = m.Type,
                Quantite = m.Quantite,
                Date = m.Date,
                MedicamentId = m.MedicamentId
            }).ToList();
        }
    }
}