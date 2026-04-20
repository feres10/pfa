using E_santeBackend.Application.DTOs.Stock;

namespace E_santeBackend.Application.Interfaces
{
    public interface IStockService
    {
        Task<bool> AjouterMouvementAsync(StockMouvementCreateDto dto);
        Task<List<StockMouvementReadDto>> GetByMedicamentIdAsync(int medicamentId);
    }

    public class StockMouvementReadDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public DateTime Date { get; set; }
        public int MedicamentId { get; set; }
    }
}