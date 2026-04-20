namespace E_santeBackend.Application.DTOs.Stock
{
    public class StockMouvementCreateDto
    {
        public string Type { get; set; } = string.Empty;
        public int Quantite { get; set; }
        public int MedicamentId { get; set; }
    }
}