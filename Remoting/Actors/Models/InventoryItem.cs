namespace Actors.Models
{
    public class InventoryItem
    {
        public Product Product { get; private set; }
        public int Stock { get; private set; }

        public InventoryItem(Product product, int initialStock)
        {
            Product = product;
            Stock = initialStock;
        }

        public int Purchase(int amount)
        {
            int backorderAmount = 0;
            if (Stock < amount)
            {
                backorderAmount = amount - Stock;
                Stock = 0;
            }
            else
            {
                Stock -= amount;
            }
            return backorderAmount;
        }
    }
}
