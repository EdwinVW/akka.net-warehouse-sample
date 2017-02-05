using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actors.Events
{
    public class ProductSold
    {
        public int ProductId { get; private set; }
        public int Amount { get; private set; }
        public decimal TotalPrice { get; private set; }

        public ProductSold(int productId, int amount, decimal totalPrice)
        {
            ProductId = productId;
            Amount = amount;
            TotalPrice = totalPrice;
        }
    }
}
