using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(
        ILogger<CategoriesController> logger,
        IRepository<CategoriesDto> repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var categories = await repo.GetAllAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Categories");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var category = await repo.FindByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving category with ID {CategoryID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync(CategoriesDto newCategory)
        {
            try
            {
                var numberCategoriesCreated = await repo.InsertAsync(newCategory);

                return Ok($"{numberCategoriesCreated} new categories created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new category");

                return StatusCode(StatusCodes.Status500InternalServerError, "Interal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, CategoriesDto updatedCategory)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Category with id {id} not found.");
                }
                var numberCategoryUpdated = await repo.UpdateAsync(id, updatedCategory);

                return Ok($"{numberCategoryUpdated} categories updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating category ID {CategoryID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                {
                    return NotFound($"Category with {id} not found");
                }

                var numberCategoryDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberCategoryDeleted} products deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting category with ID {CategoryID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
