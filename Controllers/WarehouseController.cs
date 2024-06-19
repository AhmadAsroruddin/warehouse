using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse;

using warehouse.Models;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly WarehouseRepository _repository;

    public WarehouseController(WarehouseRepository repository)
    {
        _repository = repository;
    }

    // GET api/warehouse
    [HttpGet]
    public async Task<IEnumerable<Warehouse>> Get()
    {
        return await _repository.GetAllWarehouse();
    }

    // GET api/warehouse/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Warehouse>> Get(int id)
    {
        var warehouse = await _repository.GetWarehouseById(id);
        if (warehouse == null)
        {
            return NotFound();
        }
        return warehouse;
    }

    // POST api/warehouse
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Warehouse warehouse)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _repository.AddWarehouse(warehouse);

        return CreatedAtAction(nameof(Get), new { id = warehouse.KodeGudang }, warehouse);
    }

    // PUT api/warehouse/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Warehouse warehouse)
    {
        if (id != warehouse.KodeGudang)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingWarehouse = await _repository.GetWarehouseById(id);
        if (existingWarehouse == null)
        {
            return NotFound();
        }

        await _repository.UpdateWarehouse(warehouse);

        return Ok("Data Updated Successfully");
    }

    // DELETE api/warehouse/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingWarehouse = await _repository.GetWarehouseById(id);
        if (existingWarehouse == null)
        {
            return NotFound();
        }

        await _repository.DeleteWarehouse(id);

        return Ok("warehouse with ID "+id+" deleted successfully");
    }
}
