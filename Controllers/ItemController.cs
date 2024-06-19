using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using warehouse;
using warehouse.Models;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly ItemRepository _itemRepository;

    public ItemController(ItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    // GET: api/item
    [HttpGet]
    public async Task<IEnumerable<Item>> Get()
    {
        return await _itemRepository.GetAllItems();
    }

    // GET: api/item/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> Get(int id)
    {
        var item = await _itemRepository.GetItemById(id);

        if (item == null)
        {
            return NotFound();
        }

        return item;
    }

    // POST: api/item
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Item item)
    {
        var insertedId = await _itemRepository.AddItem(item);

        return CreatedAtAction(nameof(Get), new { id = insertedId }, item);
    }

    // PUT: api/item/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Item item)
    {
        if (id != item.KodeBarang)
        {
            return BadRequest();
        }

        await _itemRepository.UpdateItem(item);

        return NoContent();
    }

    // DELETE: api/item/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingItem = await _itemRepository.GetItemById(id);

        if (existingItem == null)
        {
            return NotFound();
        }

        await _itemRepository.DeleteItem(id);

        return NoContent();
    }

    // GET: api/item/itemsbywarehouse/1
    [HttpGet("itemsbywarehouse/{warehouseId}")]
    public async Task<ActionResult<WarehouseDropdownDTO>> GetItemsByWarehouseId(int warehouseId)
    {
        var warehouseItemDTO = await _itemRepository.GetItemsByWarehouseId(warehouseId);

        if (warehouseItemDTO == null)
        {
            return NotFound();
        }

        return Ok(warehouseItemDTO);
    }

    // GET: api/item/warehouseitems/1/2023-01-01
    [HttpGet("warehouseitems/{kodeGudang}/{expiredDate}")]
    public async Task<IEnumerable<Item>> GetItemsByWarehouseAndExpiredDate(int kodeGudang, DateTime expiredDate)
    {
        return await _itemRepository.GetItemsByWarehouseAndExpiredDate(kodeGudang, expiredDate);
    }
}
