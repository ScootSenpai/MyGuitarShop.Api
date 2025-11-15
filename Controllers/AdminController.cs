using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(
        ILogger<AdminController> logger,
        IRepository<AdminDto> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var admins = await repo.GetAllAsync();

                return Ok(admins);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Admins");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var admin = await repo.FindByIdAsync(id);
                if (admin == null)
                {
                    return NotFound();
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving admin with ID {AdminID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddressAsync(AdminDto newAdmin)
        {
            try
            {
                var numberAdminsCreated = await repo.InsertAsync(newAdmin);

                return Ok($"{numberAdminsCreated} new admins created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new admin");

                return StatusCode(StatusCodes.Status500InternalServerError, "Interal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddressAsync(int id, AdminDto updatedAdmin)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Admin with id {id} not found.");
                }
                var numberAdminsUpdated = await repo.UpdateAsync(id, updatedAdmin);

                return Ok($"{numberAdminsUpdated} admins updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating admin ID {AdminID}", id);

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
                    return NotFound($"Admin with {id} not found");
                }

                var numberAdminsDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberAdminsDeleted} admins deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting admins with ID {AdminID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
