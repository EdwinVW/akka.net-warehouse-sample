namespace Messages
{
    public class SellProduct
    {
        public int ProductId { get; private set; }
        public int Amount { get; private set; }
        public decimal TotalPrice { get; private set; }

        public SellProduct(int productId, int amount, decimal totalPrice)
        {
            ProductId = productId;
            Amount = amount;
            TotalPrice = totalPrice;
        }
    }
}
