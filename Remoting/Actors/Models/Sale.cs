namespace Actors.Models
{
    public class Sale
    {
        public int ProductId { get; private set; }
        public int TotalAmount { get; private set; }
        public decimal TotalPrice { get; private set; }

        public Sale(int productId, int amount, decimal totalPrice)
        {
            ProductId = productId;
            TotalAmount = amount;
            TotalPrice = totalPrice;
        }

        public void Increase(int amount, decimal totalPrice)
        {
            TotalAmount += amount;
            TotalPrice += totalPrice;
        }
    }
}
