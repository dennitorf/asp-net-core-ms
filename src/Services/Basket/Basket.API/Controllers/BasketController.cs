using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly ILogger<BasketController> logger;
        private readonly IDiscountService discountService;
        private readonly IPublishEndpoint publishEndpoint;

        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger, IDiscountService discountService, IPublishEndpoint publishEndpoint)
        {
            this.basketRepository = basketRepository;
            this.logger = logger;
            this.discountService = discountService;
            this.publishEndpoint = publishEndpoint;
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

            // TODO: Get Discounts from Discount.Grpc

            foreach (var p in shoppingCart.ShoppingCartItems)
            {
                var coupun = await discountService.GetDiscount(p.ProdudtName);
                p.Price -= coupun.Amount;
            }
                        


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

        [HttpPost("checkout")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {            
            // get existing basket
            var basket = await basketRepository.GetBasket(basketCheckout.UserName);

            if (basket == null)
                return BadRequest();


            // create basket checkout event
            var basketCheckoutEvent = new BasketCheckoutEvent()
            {
                AddressLine = basketCheckout.AddressLine,
                CardName = basketCheckout.CardName,
                CardNumber = basketCheckout.CardNumber,
                Country = basketCheckout.Country,
                CVV = basketCheckout.CVV,
                EmailAddress = basketCheckout.EmailAddress,
                Expiration = basketCheckout.Expiration,
                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                PaymentMethod = basketCheckout.PaymentMethod,
                State = basketCheckout.State,
                TotalPrice = basket.TotalPrice,
                UserName = basketCheckout.UserName,
                ZipCode = basketCheckout.ZipCode                
            };

            await publishEndpoint.Publish(basketCheckoutEvent);

            // delete basket
            await basketRepository.DeleteBasket(basketCheckout.UserName);
            

            return Accepted();
        }

    }
}
