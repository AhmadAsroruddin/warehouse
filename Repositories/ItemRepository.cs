using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse.Models;

public class ItemRepository
{
    private readonly string _connectionString;

    public ItemRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Item>> GetAllItems()
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("SELECT * FROM Item", conn);
            var reader = await cmd.ExecuteReaderAsync();

            var items = new List<Item>();
            while (await reader.ReadAsync())
            {
                items.Add(new Item
                {
                    KodeBarang = reader.GetInt32(0),
                    NamaBarang = reader.GetString(1),
                    Harga = reader.GetDecimal(2),
                    Jumlah = reader.GetInt32(3),
                    Expired = reader.GetDateTime(4),
                    KodeGudang = reader.GetInt32(5)
                });
            }

            return items;
        }
    }

    public async Task<Item> GetItemById(int id)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("SELECT * FROM Item WHERE kode_barang = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Item
                {
                    KodeBarang = reader.GetInt32(0),
                    NamaBarang = reader.GetString(1),
                    Harga = reader.GetDecimal(2),
                    Jumlah = reader.GetInt32(3),
                    Expired = reader.GetDateTime(4),
                    KodeGudang = reader.GetInt32(5)
                };
            }

            return null;
        }
    }

    public async Task<int> AddItem(Item item)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("INSERT INTO Item (nama_barang, harga, jumlah, expired, kode_gudang) VALUES (@nama, @harga, @jumlah, @expired, @kodeGudang)", conn);
            cmd.Parameters.AddWithValue("nama", item.NamaBarang);
            cmd.Parameters.AddWithValue("harga", item.Harga);
            cmd.Parameters.AddWithValue("jumlah", item.Jumlah);
            cmd.Parameters.AddWithValue("expired", item.Expired);
            cmd.Parameters.AddWithValue("kodeGudang", item.KodeGudang);

            return await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<int> UpdateItem(Item item)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("UPDATE Item SET nama_barang = @nama, harga = @harga, jumlah = @jumlah, expired = @expired, kode_gudang = @kodeGudang WHERE kode_barang = @id", conn);
            cmd.Parameters.AddWithValue("id", item.KodeBarang);
            cmd.Parameters.AddWithValue("nama", item.NamaBarang);
            cmd.Parameters.AddWithValue("harga", item.Harga);
            cmd.Parameters.AddWithValue("jumlah", item.Jumlah);
            cmd.Parameters.AddWithValue("expired", item.Expired);
            cmd.Parameters.AddWithValue("kodeGudang", item.KodeGudang);

            return await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<int> DeleteItem(int id)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("DELETE FROM Item WHERE kode_barang = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            return await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<IEnumerable<Warehouse>> GetWarehouses()
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("SELECT kode_gudang, nama_gudang FROM Warehouse", conn);
            var reader = await cmd.ExecuteReaderAsync();

            var warehouses = new List<Warehouse>();
            while (await reader.ReadAsync())
            {
                warehouses.Add(new Warehouse
                {
                    KodeGudang = reader.GetInt32(0),
                    NamaGudang = reader.GetString(1)
                });
            }

            return warehouses;
        }
    }
}
