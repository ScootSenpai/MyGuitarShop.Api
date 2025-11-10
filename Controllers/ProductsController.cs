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
    public class ProductsController(
        ILogger<ProductsController> logger,
        IRepository<ProductDto> repo) 
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var products = await repo.GetAllAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Products");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var product = await repo.FindByIdAsync(id);
                if(product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving product with ID {ProductID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(ProductDto newProduct)
        {
            try
            {
                var numberProductsCreated = await repo.InsertAsync(newProduct);

                return Ok($"{numberProductsCreated} new products created");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error adding new product");

                return StatusCode(StatusCodes.Status500InternalServerError, "Interal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductDto updatedProduct)
        {
            try
            {
                if(await repo.FindByIdAsync(id)== null)
                {
                    return NotFound($"Product with id {id} not found.");
                }
                var numberProductsUpdated = await repo.UpdateAsync(id, updatedProduct);

                return Ok($"{numberProductsUpdated} products updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating product ID {ProductID}", id);

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
                    return NotFound($"Product with {id} not found");
                }

                var numberProductsDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberProductsDeleted} products deleted");
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message, "Error deleting product with ID {ProductID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
