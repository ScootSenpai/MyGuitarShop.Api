using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repository;
using System.Reflection.Metadata;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController(
        ILogger<AddressController> logger,
        IRepository<AddressesDto> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var addresses = await repo.GetAllAsync();

                return Ok(addresses);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Addresses");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var address = await repo.FindByIdAsync(id);
                if (address == null)
                {
                    return NotFound();
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving address with ID {AddressID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddressAsync(AddressesDto newAddress)
        {
            try
            {
                var numberAddressesCreated = await repo.InsertAsync(newAddress);

                return Ok($"{numberAddressesCreated} new addresses created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new address");

                return StatusCode(StatusCodes.Status500InternalServerError, "Interal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddressAsync(int id, AddressesDto updatedAddress)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Product with id {id} not found.");
                }
                var numberAddressesUpdated = await repo.UpdateAsync(id, updatedAddress);

                return Ok($"{numberAddressesUpdated} addresses updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating address ID {AddressID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddressAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Address with {id} not found");
                }

                var numberAddressesDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberAddressesDeleted} addresses deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting addresses with ID {AddressID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
