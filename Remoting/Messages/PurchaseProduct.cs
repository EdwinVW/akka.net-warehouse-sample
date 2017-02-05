namespace Messages
{
    public class PurchaseProduct
    {
        public int ProductId { get; private set; }
        public int Amount { get; private set; }

        public PurchaseProduct(int productId, int amount)
        {
            ProductId = productId;
            Amount = amount;
        }
    }
}
