using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(
        ILogger<CustomersController> logger,
        IRepository<CustomerDto> repo)  : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var customers = await repo.GetAllAsync();

                return Ok(customers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Customers");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var customer = await repo.FindByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving customer with ID {CustomerID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CustomerDto newCutsomer)
        {
            try
            {
                var numberCustomersCreated = await repo.InsertAsync(newCutsomer);

                return Ok($"{numberCustomersCreated} new customers created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new customer");

                return StatusCode(StatusCodes.Status500InternalServerError, "Interal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, CustomerDto updatedCustomer)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Customer with id {id} not found.");
                }
                var numberCustomersUpdated = await repo.UpdateAsync(id, updatedCustomer);

                return Ok($"{numberCustomersUpdated} customers updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating customer ID {CustomerID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Customer with {id} not found");
                }

                var numberCustomersDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberCustomersDeleted} customers deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting customer with ID {CustomerID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
