using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeeMugTestTask.API.Models;
using CoffeeMugTestTask.API.Services;

namespace CoffeeMugTestTask.API.Controllers
{
    [ApiController]
    [Route("coffemugtesttask/api")]
    public class ProductController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductsRepository productsRepository, ILogger<ProductController> logger)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            _logger.LogInformation("GetList method called.");

            IEnumerable<Product> allProducts = await _productsRepository.GetAllProductsAsync();

            if (!allProducts.Any())
            {
                return NotFound();
            }

            return Ok(allProducts);
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            _logger.LogInformation("GetProductById method called.");

            if (id.Equals(Guid.Empty))
            {
                return BadRequest();
            }

            Product product = await _productsRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product productToAdd)
        {
            _logger.LogInformation("CreateProduct method called.");

            if (productToAdd == null)
            {
                return BadRequest();
            }

            _productsRepository.CreateProduct(productToAdd);

            await _productsRepository.SaveChangesAsync();

            return  CreatedAtRoute("GetProductById", new { id = productToAdd.Id }, productToAdd.Id);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, Product updatedProduct)
        {
            _logger.LogInformation("UpdateProduct method called.");

            if (id != updatedProduct.Id)
            {
                return BadRequest();
            }

            _productsRepository.UpdateProduct(id, updatedProduct);

            await _productsRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> RemoveProduct(Guid id)
        {
            _logger.LogInformation("RemoveProduct method called.");

            if (id.Equals(Guid.Empty))
            {
                return BadRequest();
            }

            Product product = await _productsRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _productsRepository.RemoveProduct(product);

            await _productsRepository.SaveChangesAsync();

            return product;
        }
    }
}
