namespace Actors.Models
{
    public class Backorder
    {
        public int ProductId { get; private set; }
        public int Amount { get; private set; }

        public Backorder(int productId, int amount)
        {
            ProductId = productId;
            Amount = amount;
        }

        public void Increase(int amount)
        {
            Amount += amount;
        }
    }
}
