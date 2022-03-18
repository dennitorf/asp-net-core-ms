namespace Basket.API.Entities
{
    public class ShoppingCartItem
    {
        public int Quantity { set; get; }
        public string Color { set; get; }
        public decimal Price { set; get; }
        public string ProductId { set; get; }
        public string ProdudtName { set; get; }
    }
}