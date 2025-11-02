using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.Ado.Repository;
using System.Runtime.InteropServices;

namespace MyGuitarShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController(ILogger<HealthController> logger,
        SqlConnectionFactory sqlConnectionFactory) 
        : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok("Healthy");
            }
            catch(Exception)
            {
                logger.LogWarning("Health check failed unreasonably");
                return StatusCode(503, "Unhealthy");
            }
        }

        [HttpGet("db")]
        public IActionResult GetDbHealth()
        {
            try
            {
                using var connection = sqlConnectionFactory.OpenSqlConnection();
                return Ok(new {Message="Connection Succesful!", connection.Database});
            }
            catch (Exception)
            {
                logger.LogCritical("DAtabase health check failed");
                return StatusCode(503, "Database Unhealthy");
            }
        }
    }

    /*[Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
        ILogger<ProductsController> logger,
        ProductRepo repo)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await repo.GetAllProductsAsync();

                return Ok(products.Select(p => p.ProductName));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Products");

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }*/
}
