using System;
using System.Collections.Generic;
using System.Linq;

namespace Actors.Models
{
    public class Inventory
    {
        private int MIN_INITIAL_STOCK = 5;
        private const int MAX_INITIAL_STOCK = 25;
        private const double MIN_PRICE = 1.95;
        private const double MAX_PRICE = 999.95;
        private Random _rnd = new Random();
        private List<InventoryItem> _items = new List<InventoryItem>();

        public IEnumerable<InventoryItem> Items
        {
            get
            {
                return _items;
            }
        }

        public Inventory()
        {
            for (int i = 1; i <= 25; i++)
            {
                var product = new Product(
                    i,
                    $"Product #{i}",
                    Math.Round(RandomNumberBetween(MIN_PRICE, MAX_PRICE), 2));
                var inventoryItem = new InventoryItem(product, _rnd.Next(MIN_INITIAL_STOCK, MAX_INITIAL_STOCK));
                _items.Add(inventoryItem);
            }
        }

        private decimal RandomNumberBetween(double minValue, double maxValue)
        {
            var next = _rnd.NextDouble();

            return (decimal)(minValue + (next * (maxValue - minValue)));
        }
    }
}
