using warehouse.Models;

namespace warehouse;

public class WarehouseDropdownDTO
{
     public int KodeGudang { get; set; }
        public required string NamaGudang { get; set; }
        public required List<Item> Items { get; set; }
}
