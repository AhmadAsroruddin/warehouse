using warehouse.Models
;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WarehouseRepository
{
   private readonly string _connectionString;

    public WarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Warehouse>> GetAllWarehouse()
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("SELECT * FROM Warehouse", conn);
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

    public async Task<Warehouse> GetWarehouseById(int id)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("SELECT * FROM Warehouse WHERE kode_gudang = @id", conn);
            cmd.Parameters.AddWithValue("id", id);
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Warehouse
                {
                    KodeGudang = reader.GetInt32(0),
                    NamaGudang = reader.GetString(1)
                };
            }

            return null;
        }
    }

    public async Task<int> AddWarehouse(Warehouse warehouse)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("INSERT INTO Warehouse (nama_gudang) VALUES (@nama)", conn);
            cmd.Parameters.AddWithValue("nama", warehouse.NamaGudang);

            return await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<int> UpdateWarehouse(Warehouse warehouse)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("UPDATE Warehouse SET nama_gudang = @nama WHERE kode_gudang = @id", conn);
            cmd.Parameters.AddWithValue("id", warehouse.KodeGudang);
            cmd.Parameters.AddWithValue("nama", warehouse.NamaGudang);

            return await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<int> DeleteWarehouse(int id)
    {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand("DELETE FROM Warehouse WHERE kode_gudang = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
