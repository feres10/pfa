using E_santeBackend.Application.DTOs.Medicament;
using E_santeBackend.Application.Interfaces;
using E_santeBackend.Infrastructure.Repositories;

namespace E_santeBackend.Application.Services
{
    public class MedicamentService : IMedicamentService
    {
        private readonly MedicamentRepository _repository;

        public MedicamentService(MedicamentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MedicamentReadDto>> GetAllAsync()
        {
            var medicaments = await _repository.GetAllAsync();
            return medicaments.Select(m => new MedicamentReadDto
            {
                Id = m.Id,
                Nom = m.Nom,
                Dosage = m.Dosage,
                Fabricant = m.Fabricant,
                Stock = m.Stock,
                DateExpiration = m.DateExpiration
            }).ToList();
        }

        public async Task<MedicamentReadDto?> GetByIdAsync(int id)
        {
            var medicament = await _repository.GetByIdAsync(id);
            if (medicament == null) return null;

            return new MedicamentReadDto
            {
                Id = medicament.Id,
                Nom = medicament.Nom,
                Dosage = medicament.Dosage,
                Fabricant = medicament.Fabricant,
                Stock = medicament.Stock,
                DateExpiration = medicament.DateExpiration
            };
        }

        public async Task<List<MedicamentReadDto>> GetExpiringSoonAsync()
        {
            var medicaments = await _repository.GetExpiringSoonAsync();
            return medicaments.Select(m => new MedicamentReadDto
            {
                Id = m.Id,
                Nom = m.Nom,
                Dosage = m.Dosage,
                Fabricant = m.Fabricant,
                Stock = m.Stock,
                DateExpiration = m.DateExpiration
            }).ToList();
        }

        public async Task<MedicamentReadDto> CreateAsync(E_santeBackend.Application.DTOs.Medicament.MedicamentCreateDto dto)
        {
            // Ensure DateExpiration is stored as UTC to satisfy PostgreSQL timestamptz expectations
            var dateExp = dto.DateExpiration;
            if (dateExp.Kind == DateTimeKind.Unspecified)
            {
                // Treat unspecified as UTC to avoid Npgsql error when writing to timestamptz
                dateExp = DateTime.SpecifyKind(dateExp, DateTimeKind.Utc);
            }
            else
            {
                dateExp = dateExp.ToUniversalTime();
            }

            var entity = new Domain.Entities.Medicament
            {
                Nom = dto.Nom,
                Dosage = dto.Dosage,
                Fabricant = dto.Fabricant,
                Stock = dto.Stock,
                DateExpiration = dateExp
            };

            await _repository.AddAsync(entity);

            return new MedicamentReadDto
            {
                Id = entity.Id,
                Nom = entity.Nom,
                Dosage = entity.Dosage,
                Fabricant = entity.Fabricant,
                Stock = entity.Stock,
                DateExpiration = entity.DateExpiration
            };
        }
    }
}