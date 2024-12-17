namespace OrderingAPI.Events
{
    public class BasketCheckoutEvent
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public List<BasketItem> Items { get; set; }
    }

    public class BasketItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
