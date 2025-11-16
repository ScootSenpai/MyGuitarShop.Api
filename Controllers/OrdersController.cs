using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(
        ILogger<OrdersController> logger,
        IRepository<OrderDto> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var orders = await repo.GetAllAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Orders");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var order = await repo.FindByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving order with ID {OrderID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(OrderDto newOrder)
        {
            try
            {
                var numberOrdersCreated = await repo.InsertAsync(newOrder);

                return Ok($"{numberOrdersCreated} new orders created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new order");

                return StatusCode(StatusCodes.Status500InternalServerError, "Interal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, OrderDto updatedOrder)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Order with id {id} not found.");
                }
                var numberOrdersUpdated = await repo.UpdateAsync(id, updatedOrder);

                return Ok($"{numberOrdersUpdated} orders updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating order ID {OrderID}", id);

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
                    return NotFound($"Order with {id} not found");
                }

                var numberOrdersDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberOrdersDeleted} orders deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting order with ID {OrderID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
