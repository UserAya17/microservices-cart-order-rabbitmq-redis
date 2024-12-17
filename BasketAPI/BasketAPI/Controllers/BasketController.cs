
using BasketAPI.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class BasketController : Controller
{
    private readonly IBasketRepository _basketRepository;

    public BasketController(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userName = "testUser";
        var basket = await _basketRepository.GetBasket(userName);

        if (basket == null || !basket.Items.Any())
        {
            // Seed test data with images
            basket = new ShoppingCart(userName)
            {
                Items = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ProductId = 1,
                    ProductName = "Flower a",
                    Quantity = 1,
                    Price = 199.99m,
                    ImageUrl = "/images/f1.jpg"
                },
                new ShoppingCartItem
                {
                    ProductId = 2,
                    ProductName = "Flower b",
                    Quantity = 1,
                    Price = 1111.99m,
                    ImageUrl = "/images/f2.jpg"
                },
                new ShoppingCartItem
                {
                    ProductId = 3,
                    ProductName = "Flower c",
                    Quantity = 1,
                    Price = 822.99m,
                    ImageUrl = "/images/f2.jpg"
                },
                new ShoppingCartItem
                {
                    ProductId = 4,
                    ProductName = "Flower d",
                    Quantity = 1,
                    Price = 300.99m,
                    ImageUrl = "/images/f3.jpg"
                },
                new ShoppingCartItem
                {
                    ProductId = 5,
                    ProductName = "Flower e",
                    Quantity = 1,
                    Price = 100.9m,
                    ImageUrl = "/images/f4.jpg"
                }
            }
            };

            await _basketRepository.UpdateBasket(basket);
        }

        return View(basket.Items);
    }



    [HttpPost("updateQuantity/{productId}")]
    public async Task<IActionResult> UpdateQuantity(string productId, int change)
    {
        var userName = "testUser"; // Replace with logged-in user
        var basket = await _basketRepository.GetBasket(userName);

        if (basket == null || basket.Items == null)
        {
            return NotFound("Basket not found.");
        }

        if (int.TryParse(productId, out var parsedProductId))
        {
            var item = basket.Items.FirstOrDefault(i => i.ProductId == parsedProductId);
            if (item != null)
            {
                // Update quantity incrementally
                item.Quantity += change;

                // Prevent negative quantities
                if (item.Quantity < 1)
                {
                    item.Quantity = 1; // Set to minimum of 1 or handle accordingly
                }
            }
            else
            {
                return NotFound("Item not found in the basket.");
            }
        }
        else
        {
            return BadRequest("Invalid productId.");
        }

        await _basketRepository.UpdateBasket(basket);
        return Ok();
    }

    [HttpDelete("removeItem/{productId}")]
    public async Task<IActionResult> RemoveItem(string productId)
    {
        var userName = "testUser"; // Replace with logged-in user logic
        var basket = await _basketRepository.GetBasket(userName);

        if (basket == null || basket.Items == null)
        {
            return NotFound("Basket not found.");
        }

        if (int.TryParse(productId, out var parsedProductId))
        {
            // Remove the item entirely
            var itemToRemove = basket.Items.FirstOrDefault(i => i.ProductId == parsedProductId);
            if (itemToRemove != null)
            {
                basket.Items.Remove(itemToRemove);
            }
            else
            {
                return NotFound("Item not found in the basket.");
            }
        }
        else
        {
            return BadRequest("Invalid productId.");
        }

        await _basketRepository.UpdateBasket(basket);
        return Ok();
    }
    [HttpPost("decrementQuantity/{productId}")]
    public async Task<IActionResult> DecrementQuantity(string productId)
    {
        var userName = "testUser"; // Replace with logged-in user logic
        var basket = await _basketRepository.GetBasket(userName);

        if (basket == null || basket.Items == null)
        {
            return NotFound("Basket not found.");
        }

        if (int.TryParse(productId, out var parsedProductId))
        {
            var item = basket.Items.FirstOrDefault(i => i.ProductId == parsedProductId);
            if (item != null)
            {
                // Decrease the quantity by 1
                item.Quantity -= 1;

                // Remove the item entirely if quantity drops to 0
                if (item.Quantity <= 0)
                {
                    basket.Items.Remove(item);
                }

                await _basketRepository.UpdateBasket(basket);
                return Ok(item);
            }
            else
            {
                return NotFound("Item not found in the basket.");
            }
        }
        else
        {
            return BadRequest("Invalid productId.");
        }
    }


}
