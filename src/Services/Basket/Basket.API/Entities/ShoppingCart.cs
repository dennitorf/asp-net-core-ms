using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { set; get; }
        public List<ShoppingCartItem> ShoppingCartItems { set; get; } = new List<ShoppingCartItem>();

        public decimal TotalPrice
        {
            get
            {
                return ShoppingCartItems
                    .Sum(c => c.Quantity * c.Price);
            }
        }
    }
}
