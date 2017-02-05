namespace Messages
{
    public class BackorderProduct
    {
        public int ProductId { get; private set; }
        public int Amount { get; private set; }

        public BackorderProduct(int productId, int amount)
        {
            ProductId = productId;
            Amount = amount;
        }
    }
}
