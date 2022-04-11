using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using System.Threading.Tasks;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BasketCheckoutConsumer> logger;

        public BasketCheckoutConsumer(IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
        {
            _mediator = mediator;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            
            var result = await _mediator.Send(new CheckoutOrderCommand() 
            { 
                AddressLine = context.Message.AddressLine,
                CardName = context.Message.CardName,
                CardNumber = context.Message.CardNumber,
                Country = context.Message.Country,
                CVV = context.Message.CVV,
                EmailAddress = context.Message.EmailAddress,
                Expiration = context.Message.Expiration,
                FirstName = context.Message.FirstName,
                LastName = context.Message.LastName,    
                PaymentMethod = context.Message.PaymentMethod,
                State = context.Message.State,
                TotalPrice = context.Message.TotalPrice,
                UserName = context.Message.UserName,
                ZipCode = context.Message.ZipCode
            });

            logger.LogInformation("A Order : {result} is created", result);
        }
    }
}
