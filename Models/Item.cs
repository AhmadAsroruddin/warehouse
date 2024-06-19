using System;
namespace warehouse.Models;

public class Item
{
    public int KodeBarang { get; set; }
    public string NamaBarang { get; set; }
    public decimal Harga { get; set; }
    public int Jumlah { get; set; }
    public DateTime expired { get; set; }
    public int KodeGudang { get; set; }
}
