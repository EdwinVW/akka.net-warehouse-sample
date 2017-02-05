using Akka.Actor;
using ColoredConsole;
using Messages;
using Actors.Models;
using System;

namespace Actors
{
    public class ProductActor : TypedActor, IHandle<PurchaseProduct>
    {
        private Product _product;
        private int _stock;
        ActorSelection _backorderActor;
        ActorSelection _salesActor;

        public ProductActor(Product product, int currentStock)
        {
            _product = product;
            _stock = currentStock;
            _backorderActor = Context.ActorSelection("/user/backorder");
            _salesActor = Context.ActorSelection("/user/sales");
        }

        public void Handle(PurchaseProduct message)
        {
            // determine backorder
            int backorderAmount = PurchaseProduct(message.Amount);

            // show status
            int purchased = message.Amount - backorderAmount;
            ColorConsole.WriteLine($"ProductActor for product {message.ProductId}: purchased {purchased} units.".Green());

            // handle sales
            decimal totalPrice = CalculatePrice(purchased);
            _salesActor.Tell(new SellProduct(message.ProductId, purchased, totalPrice));

            // handle backorder
            if (backorderAmount > 0)
            {
                _backorderActor.Tell(new BackorderProduct(message.ProductId, backorderAmount));
            }

            // update scanner
            Sender.Tell(new ProductPurchased(message.ProductId, purchased, backorderAmount));
        }

        private int PurchaseProduct(int amount)
        {
            int backorderAmount = 0;
            if (_stock < amount)
            {
                backorderAmount = amount - _stock;
                _stock = 0;
            }
            else
            {
                _stock -= amount;
            }
            return backorderAmount;
        }

        private decimal CalculatePrice(int amount)
        {
            return amount * _product.Price;
        }
    }
}
