using MassTransit;
using OrderingAPI.Data;
using OrderingAPI.Events;
using OrderingAPI.Models;

namespace OrderingAPI.Consumers
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly OrderingContext _context;

        public BasketCheckoutConsumer(OrderingContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var message = context.Message;

            // Map the message to an Order entity
            var order = new Order
            {
                UserName = message.UserName,
                TotalPrice = message.TotalPrice,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                Address = message.Address,
                PaymentMethod = message.PaymentMethod
            };

            // Save the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
