using BasketAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        // Constructor to inject Redis distributed cache
        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        // Get basket for a specific user
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("User name must not be null or empty", nameof(userName));

            var basket = await _redisCache.GetStringAsync(userName);

            return string.IsNullOrEmpty(basket)
                ? new ShoppingCart(userName) // Return empty shopping cart if no data is found
                : JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        // Update (or create) basket for a specific user
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            if (basket == null)
                throw new ArgumentNullException(nameof(basket));

            if (string.IsNullOrEmpty(basket.UserName))
                throw new ArgumentException("Basket must have a valid user name", nameof(basket.UserName));

            var serializedBasket = JsonSerializer.Serialize(basket);

            await _redisCache.SetStringAsync(basket.UserName, serializedBasket);

            return basket;
        }

        // Delete basket for a specific user
        public async Task DeleteBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("User name must not be null or empty", nameof(userName));

            await _redisCache.RemoveAsync(userName);
        }
    }
}
