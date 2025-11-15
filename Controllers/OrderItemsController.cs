using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController(
        ILogger<OrderItemsController> logger,
        IRepository<OrderItemDto> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var orderitems = await repo.GetAllAsync();

                return Ok(orderitems);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Order Items");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var orderitem = await repo.FindByIdAsync(id);
                if (orderitem == null)
                {
                    return NotFound();
                }
                return Ok(orderitem);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving order item with ID {ItemID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(OrderItemDto newOrderItem)
        {
            try
            {
                var numberOrderItemsCreated = await repo.InsertAsync(newOrderItem);

                return Ok($"{numberOrderItemsCreated} new order items created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new order item");

                return StatusCode(StatusCodes.Status500InternalServerError, "Interal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, OrderItemDto updatedOrderItem)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Order item with id {id} not found.");
                }
                var numberOrderItemsUpdated = await repo.UpdateAsync(id, updatedOrderItem);

                return Ok($"{numberOrderItemsUpdated} order items updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating order item ID {ItemID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Order Item with {id} not found");
                }

                var numberOrderItemsDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberOrderItemsDeleted} order items deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting order items with ID {ItemID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
