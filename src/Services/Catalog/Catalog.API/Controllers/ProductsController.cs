using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository productRepository;
        private ILogger<ProductsController> logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<IActionResult> Get()
        {
            logger.LogInformation("GET Products Started");
            return Ok(await productRepository.GetAll());            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute]string id)
        {
            logger.LogInformation("GET Products By Id Started : {id}", id);
            var product = await productRepository.GetById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }


        [HttpGet("GetByName/{name}")]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            logger.LogInformation("GET ProductsByName Started: {0}", name);
            return Ok(await productRepository.GetByName(name));
        }

        [HttpGet("GetByCategory/{category}")]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<IActionResult> GetByCategory([FromRoute] string category)
        {
            logger.LogInformation("GET ProductsByCategory Started: {0}", category);
            return Ok(await productRepository.GetByCategory(category));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<IActionResult> Post(Product product)
        {
            logger.LogInformation("POST Products Started: {0}", product.Id);
            return Ok(await productRepository.Create(product));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<IActionResult> Put(Product product)
        {
            logger.LogInformation("PUT Products Started: {0}", product.Id);
            return Ok(await productRepository.Update(product));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            logger.LogInformation("DELETE Products By Id Started : {id}", id);
            await productRepository.DeleteById(id);            
            return Ok();
        }



    }
}
