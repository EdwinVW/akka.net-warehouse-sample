using Akka.Actor;
using Actors.Models;
using Messages;
using System;
using System.Collections.Generic;

namespace Actors
{
    public class InventoryActor : ReceiveActor
    {
        private Inventory _inventory;
        private List<IActorRef> _productActors = new List<IActorRef>();

        public InventoryActor()
        {
            _inventory = new Inventory();

            // start a product actor for each inventoryitem
            foreach (InventoryItem item in _inventory.Items)
            {
                string actorName = $"product-{item.Product.Id}";
                Props props = new Props(typeof(ProductActor), new object[] { item.Product, item.Stock });
                _productActors.Add(Context.ActorOf(props, actorName));
            }
        }
    }
}
