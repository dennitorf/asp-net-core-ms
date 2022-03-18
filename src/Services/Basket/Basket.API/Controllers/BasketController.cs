using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly ILogger<BasketController> logger;

        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger)
        {
            this.basketRepository = basketRepository;
            this.logger = logger;
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<IActionResult> Get(string userName)
        {
            logger.LogInformation("GET Basket Started {userName}", userName);
            var basket = await basketRepository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart() {  UserName = userName });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<IActionResult> Put(ShoppingCart shoppingCart)
        {
            logger.LogInformation("PUT Basket Started {userName}", shoppingCart.UserName);
            var basket = await basketRepository.UpdateBasket(shoppingCart);
            return Ok(basket);
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
        public async Task<IActionResult> Delete(string userName)
        {
            logger.LogInformation("DELETE Basket Started {userName}", userName);
            await basketRepository.DeleteBasket(userName);
            return Ok();
        }



    }
}
