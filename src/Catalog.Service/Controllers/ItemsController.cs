using Microsoft.AspNetCore.Mvc;

namespace Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private static List<ItemDto> items = new ()
    {
        new ItemDto(Guid.NewGuid(), "Item 1", "This is item 1", 10, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Item 2", "This is item 2", 20, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Item 3", "This is item 3", 30, DateTimeOffset.UtcNow)
    };

    private static List<ItemDto> GetItems()
    {
        return items;
    }

    private static void SetItems(List<ItemDto> value)
    {
        items = value;
    }

    [HttpGet]
    public IEnumerable<ItemDto> GetAll()
    {
        return GetItems();
    }

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetById(Guid id)
    {
        var item = GetItems().Where(x => x.Id == id).FirstOrDefault();

        if (item == null)
        {
            return NotFound();
        }

        return item;
    }

    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
    {
        var item = new ItemDto(
            Guid.NewGuid(),
            createItemDto.Name,
            createItemDto.Description,
            createItemDto.Price,
            DateTimeOffset.UtcNow);

        items.Add(item);

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

        if (existingItem == null)
        {
            return NotFound();
        }

        var updateItem = existingItem with
        {
            Name = updateItemDto.Name,
            Description = updateItemDto.Description,
            Price = updateItemDto.Price
        };

        var index = items.FindIndex(x => x.Id == id);
        items[index] = updateItem;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var index = items.FindIndex(x => x.Id == id);

        if (index < 0)
        {
            return NotFound();
        }

        items.RemoveAt(index);

        return NoContent();
    }
}
